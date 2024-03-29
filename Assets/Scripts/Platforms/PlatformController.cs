﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {

	public LayerMask passengerMask; 

	public Vector3 [] localWaypoints; 
	Vector3[] globalWaypoints; 

	public bool cyclicPlatform; 
	public float speed; 
	public float platformWaitTime; 
	[Range(0,22)]
	public float platformEaseAmount; 

	float percentBetweenWaypoints; // Percentage between 0 and 1 
	float nextPlatformMoveTime; 

	int fromWaypointIndex; 


	List <PassengerMovement> passengerMovement; 
	Dictionary<Transform, Transform> passengerDictionary = new Dictionary<Transform, Transform>(); 


	public override void Start () {
		base.Start(); 

		 globalWaypoints = new Vector3[localWaypoints.Length];
		 for(int i=0; i < localWaypoints.Length; i++){
		 globalWaypoints[i] = localWaypoints[i] + transform.position; 
		 }
	}
	
	void Update (){

		UpdateRaycastOrigin();

		Vector3 velocity = CalculatePlatformMovement();

		CalculatePassengerMovement (velocity); 
		MovePassenger(true);
		transform.Translate (velocity); 
		MovePassenger(false);

		}


	// In concern to easing with platforms - the higher the number the faster the platform will accelerate.  Generally for a usable platform 
	// an ease amount of 1-3 would be ideal, but for use on hazards or other things higher numbers could be experimented with.  


	float PlatformEase (float x){
		float a = platformEaseAmount + 1; 
		return Mathf.Pow (x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1-x,a)); 
	}

	Vector3 CalculatePlatformMovement ()
	{

	// Time.time is time in seconds. So time to wait for the platforms will be in seconds. 

		if (Time.time < nextPlatformMoveTime) {
			return Vector3.zero; 
		}

		fromWaypointIndex %= globalWaypoints.Length; 
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length; 
		float distanceBetweenWaypoints = Vector3.Distance (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex]); 
		percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01 (percentBetweenWaypoints); 
		float easedPercentBetweenWaypoints = PlatformEase(percentBetweenWaypoints); 

		Vector3 newPosition = Vector3.Lerp (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex], easedPercentBetweenWaypoints); 
		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0; 
			fromWaypointIndex++; 
			if (!cyclicPlatform) {
				if (fromWaypointIndex >= globalWaypoints.Length - 1) {
					fromWaypointIndex = 0; 
					System.Array.Reverse (globalWaypoints); 
				}
			}

			nextPlatformMoveTime = Time.time + platformWaitTime; 
		}  
		return newPosition - transform.position; 

	}

//  For collison mask select 'Nothing' and for Passenger mask, select 'Player' so the Platform will carry the player. 
	void MovePassenger (bool beforeMovePlatform)
	{
		foreach (PassengerMovement passenger in passengerMovement) {
			if (!passengerDictionary.ContainsKey (passenger.transform)) {
				passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Transform>());
				
			}
			if (passenger.moveBeforePlatform == beforeMovePlatform) {
				passenger.transform.GetComponent<Controller2D>().Move(passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}

	void CalculatePassengerMovement (Vector3 velocity)
	{
		HashSet<Transform> movedPassengers = new HashSet<Transform> (); 
		passengerMovement = new List<PassengerMovement> (); 

		float directionX = Mathf.Sign (velocity.x); 
		float directionY = Mathf.Sign (velocity.y); 


		// Vertically moving platform

		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y) + skinWidth;
			
			for (int i = 0; i < verticalRaycount; i++) {
				Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						float pushX = (directionY == 1) ? velocity.x : 0;
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true)); 
					}
				}
			}
		}
		// Horizontally moving platforms

		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x) + skinWidth; 

			for (int i = 0; i < horizontalRayCount; i++) { 
				Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains (hit.transform)) {
						movedPassengers.Add (hit.transform);
						float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
						float pushY = -skinWidth * 2;
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true)); 
					}

				}

			}
		}

		// On top of a horizontally or downward moving platform

		if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = skinWidth * 2;
			
			for (int i = 0; i < verticalRaycount; i++) {
				Vector2 rayOrigin = raycastOrigin.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up, rayLength, passengerMask);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains (hit.transform)) {
						movedPassengers.Add (hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;

						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false)); 
					}
				}
			}
		}
	}

	struct PassengerMovement {
		public Transform transform; 
		public Vector3 velocity; 
		public bool standingOnPlatform; 
		public bool moveBeforePlatform; 

		public PassengerMovement (Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
			transform = _transform;
			velocity = _velocity; 
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform; 
		}
	}
	void OnDrawGizmos (){
		if (localWaypoints !=null){
			Gizmos.color = Color.red; 
			float size = .3f; 

			for (int i = 0; i < localWaypoints.Length; i++) {
				Vector3 globalWaypointPosition = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i] + transform.position; 
				Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size); 
				Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size); 
			 } 
		 } 
	}
}
