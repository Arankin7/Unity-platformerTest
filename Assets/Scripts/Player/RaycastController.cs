using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (BoxCollider2D))]

public class RaycastController : MonoBehaviour {

	public LayerMask collisionMask; 

	public const float skinWidth = .015f;

	[HideInInspector]
	public float horizontalRaySpacing;
	[HideInInspector]
	public float verticalRaySpacing; 

	const float dstBetweenRays = .1f; 
	[HideInInspector]
	public int horizontalRayCount;
	[HideInInspector]
	public int verticalRaycount; 

	public BoxCollider2D collsions; 
	public RaycastOrigin raycastOrigin; 

	public virtual void Awake () {
		collsions = GetComponent<BoxCollider2D> (); 
	}

	public virtual void Start ()
	{
		CalculateRaySpacing ();

	}

	public void UpdateRaycastOrigin (){
		Bounds bounds = collsions.bounds;
		bounds.Expand (skinWidth * -2); 

		raycastOrigin.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigin.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigin.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigin.topRight = new Vector2 (bounds.max.x, bounds.max.y);
		 
	}

	// Calclulates the amount of rays and the spacing between rays. 

	public void CalculateRaySpacing (){
		Bounds bounds = collsions.bounds;
		bounds.Expand (skinWidth * -2); 

		float boundsWidth = bounds.size.x; 
		float boundsHeight = bounds.size.y; 

		horizontalRayCount = Mathf.RoundToInt (boundsHeight/dstBetweenRays); 
		verticalRaycount = Mathf.RoundToInt (boundsWidth/dstBetweenRays); 

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount -1); 
		verticalRaySpacing = bounds.size.x / (verticalRaycount -1);

	}

	public struct RaycastOrigin {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;

	}
}
