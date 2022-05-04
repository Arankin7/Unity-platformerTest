using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller2D : RaycastController {

	
	public float maxSlopeAngle = 60; 


	public CollisionInfo collisions; 
	Vector2 playerInput; 
	 

	public override void Start (){
		base.Start (); 
		collisions.faceDirection = 1; 

	}

	public void Move (Vector2 moveSpeed, bool standingOnPlatform){
		Move (moveSpeed, Vector2.zero, standingOnPlatform); 

	}
		

	public void Move (Vector2 moveSpeed, Vector2 input, bool standingOnPlatform = false)
	{
		UpdateRaycastOrigin (); 
		collisions.Reset ();
		collisions.moveSpeedOld = moveSpeed; 
		playerInput = input; 


		if (moveSpeed.y < 0) {
			DescendSlope (ref moveSpeed); 
		}
		if (moveSpeed.x !=0) {
			collisions.faceDirection = (int)Mathf.Sign(moveSpeed.x); 
		}
	
			HorizontalCollisons (ref moveSpeed);

		if (moveSpeed.y != 0) {
			VertcialCollisons (ref moveSpeed); 
		}
		transform.Translate (moveSpeed); 
		if (standingOnPlatform) {
			collisions.below = true; 
		}
	}



	void HorizontalCollisons (ref Vector2 moveSpeed)
	{
		float directionX = collisions.faceDirection;
		float rayLength = Mathf.Abs (moveSpeed.x) + skinWidth; 

		if (Mathf.Abs (moveSpeed.x) < skinWidth) {
			rayLength = 2 * skinWidth; 
		}

		for (int i = 0; i < horizontalRayCount; i++) { 
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.right * directionX, Color.red);

			if (hit) {
				if (hit & gameObject.CompareTag ("Breakable")) {
					StartCoroutine ("BreakPlat");
				}
				if (hit & gameObject.CompareTag ("Respawn")){
					StartCoroutine("PlatRespawn");
				 }	 
				
				if (hit.distance == 0) {
					continue;
				}

				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= maxSlopeAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						moveSpeed = collisions.moveSpeedOld; 
					}
					float distanceToSlopeStart = 0; 
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance - skinWidth; 
						moveSpeed.x -= distanceToSlopeStart * directionX; 
					}
					ClimbSlope (ref moveSpeed, slopeAngle, hit.normal);
					moveSpeed.x += distanceToSlopeStart * directionX; 

				}
				if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle) {

					moveSpeed.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance; 
					if (collisions.climbingSlope) {
						moveSpeed.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveSpeed.x);
					}
					collisions.left = directionX == -1;
					collisions.right = directionX == 1; 
				}
 			}
		}

	}

	void VertcialCollisons (ref Vector2 moveSpeed)
	{
		float directionY = Mathf.Sign (moveSpeed.y);
		float rayLength = Mathf.Abs (moveSpeed.y) + skinWidth; 

		for (int i = 0; i < verticalRaycount; i++) { 
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + moveSpeed.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.up * directionY, Color.red);

			if (hit) {
				if (hit & gameObject.CompareTag ("Breakable")){
					StartCoroutine("BreakPlat");
				 }	
				if (hit & gameObject.CompareTag ("Respawn")){
					StartCoroutine("PlatRespawn");
				 }	 
				
				if (hit.collider.tag == "Through") {
					if (directionY == 1 || hit.distance == 0) {
						continue; 
					}
					if (collisions.fallingThroughPlatform) {
						continue; 
					}
					if (playerInput.y == -1) {
						collisions.fallingThroughPlatform = true; 
						Invoke ("ResetFallingThroughPlat", .3f); 
						continue; 
					}
				}
				moveSpeed.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance; 

				if (collisions.climbingSlope) {
					moveSpeed.x = moveSpeed.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (moveSpeed.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1; 
			}
		}

		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign (moveSpeed.x);
			rayLength = Mathf.Abs (moveSpeed.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight) + Vector2.up * moveSpeed.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask); 

			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up); 
				if (slopeAngle != collisions.slopeAngle) {
					moveSpeed.x = (hit.distance - skinWidth) * directionX; 
					collisions.slopeAngle = slopeAngle; 
					collisions.slopeNormal = hit.normal; 

				} 
			}	  
		}
	}

	void ClimbSlope (ref Vector2 moveSpeed, float slopeAngle, Vector2 slopeNormal)
	{
		float moveDistance = Mathf.Abs (moveSpeed.x); 
		float climbmoveSpeedY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		if (moveSpeed.y <= climbmoveSpeedY) {
			moveSpeed.y = climbmoveSpeedY; 
			moveSpeed.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveSpeed.x); 
			collisions.below = true; 
			collisions.climbingSlope = true; 
			collisions.slopeAngle = slopeAngle; 
			collisions.slopeNormal = slopeNormal; 

		}
	}


	void DescendSlope (ref Vector2 moveSpeed)
	{
		RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast (raycastOrigin.bottomLeft, Vector2.down, Mathf.Abs (moveSpeed.y) + skinWidth, collisionMask); 
		RaycastHit2D maxSlopeHitRight = Physics2D.Raycast (raycastOrigin.bottomRight, Vector2.down, Mathf.Abs (moveSpeed.y) + skinWidth, collisionMask);
		if (maxSlopeHitLeft ^ maxSlopeHitRight) {
			SlideDownMaxSlope (maxSlopeHitLeft, moveSpeed); 
			SlideDownMaxSlope (maxSlopeHitRight, moveSpeed); 

		}
		if (!collisions.slidingDownMaxSlope){
			float directionX = Mathf.Sign (moveSpeed.x); 
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomRight : raycastOrigin.bottomLeft; 
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask); 

			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up); 
				if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle) {
					if (Mathf.Sign (hit.normal.x) == directionX) {
						if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveSpeed.x)){
							float moveDistance = Mathf.Abs (moveSpeed.x); 
							float descendmoveSpeedY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance; 
							moveSpeed.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveSpeed.x); 
							moveSpeed.y -= descendmoveSpeedY; 

							collisions.slopeAngle = slopeAngle; 
							collisions.descendingSlope = true; 
							collisions.below = true;  
							collisions.slopeNormal = hit.normal; 

						}
					}

				}
			}	 
		}
	}

	void SlideDownMaxSlope (RaycastHit2D hit, Vector2 moveSpeed)
	{
		if (hit) {
			float slopeAngle = Vector2.Angle (hit.normal, Vector2.up); 
			if(slopeAngle > maxSlopeAngle) {
				moveSpeed.x = Mathf.Sign(hit.normal.x) *(Mathf.Abs (moveSpeed.y) - hit.distance)/ Mathf.Tan (slopeAngle * Mathf.Deg2Rad); 

				collisions.slopeAngle = slopeAngle; 
				collisions.slidingDownMaxSlope = true; 
				collisions.slopeNormal = hit.normal; 
			} 
		}
	}

	void ResetFallingThroughPlat (){
		collisions.fallingThroughPlatform = false; 
	}


	public struct CollisionInfo {
	
		public bool above, below;
		public bool left, right; 
		public bool climbingSlope; 
		public bool descendingSlope; 
		public bool fallingThroughPlatform; 
		public bool slidingDownMaxSlope; 

		public float slopeAngle, slopeAngleOld; 
		public Vector2 slopeNormal; 
		public int faceDirection; 
		public Vector2 moveSpeedOld; 

		public void Reset (){
			above = false;
			below = false;
			left = false;
			right = false;
			climbingSlope = false; 
			descendingSlope = false; 
			slidingDownMaxSlope = false; 

			slopeNormal = Vector2.zero; 
			slopeAngleOld = slopeAngle; 
			slopeAngle = 0; 
	
		}
	}
}
