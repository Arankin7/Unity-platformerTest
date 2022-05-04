using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))] 

public class Player : MonoBehaviour {


	public float maxJumpHeight = 6f; 
	public float minJumpHeight = 1f; 
	public float timeToJumpApex = .5f; 

	public float dashVelocitySmooth = 1f; 
	public float airDashSpeed = 10f; 
	public float maxAirDashDist = 10f; 
	public float minAirDashDist = 0f; 

	public float movespeed = 6f;
    public float maxMovespeed; 
    //public float runspeed = 20f; 
	public float accelerationTimeAirborn = .2f; 
	public float accelerationTimeGrounded= .1f; 
	public float wallClingSpeedMax = 3f; 
	public float wallClingTime = .25f; 

	public Vector2 WallJump; 

	float gravity;  
	float maxJumpVelocity;
	float minJumpVelocity; 
	float velocityXSmoothing; 
	float timeToWallUncling; 


	public Vector3 velocity; 

	Controller2D controller; 

	Vector2 directionalInput; 

	bool airBorne;  
	bool wallCling; 
	int wallDirectionX; 

	int maxDashes = 1; 
	int dashCount = 0; 


	void Start () {
		controller = GetComponent<Controller2D> (); 

		// gravity = (2 * jumpHeight) / (timeToJumpApex²)
		// Gravity should be negative, hence the - sign in front of the equation.
		 
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

		// jumpVelocity = gravity * timeToJumpApex 
		// Mathf.abs is used to use the absolute (positive value) of gravity, since it is a negative value initially. 

		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex); 
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);

		//print ("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);  

	}
	void Update ()
	{
		

		CalculateVelocity (); 

		HandleWallCling (); 

		HandleDashing ();

       // maxMovespeed = movespeed; 

		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		SetDirectionalInput (directionalInput); 

        //    Trying to get running Working.  This is my best guess so far.  WIP.     
        //
        //  if (Input.GetKeyDown (KeyCode.LeftShift))
        //{
          //  OnRunInputDown() ; }

        //if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
          //  OnRunInputDown() ;
        //}


        if (Input.GetKeyDown (KeyCode.Space)) {
			OnJumpInputDown (); 
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			OnJumpInputUp (); 
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			OnDashInputDown ();
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			OnDashInputUp ();
		}


		controller.Move (velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0; 

			}
		}
	}
    //   Will work on running SOON.  High priority. 
   // public void OnRunInputDown() {

     //   maxMovespeed = runspeed ;  
    //}

//    public void OnRunInputUp() {
  //      maxMovespeed = movespeed; 
    //}

	public void SetDirectionalInput (Vector2 input)
	{

		directionalInput = input;
	}


	public void OnDashInputDown ()
	{
        GetComponent<Controller2D>();
		if  (directionalInput.x > 0 && airBorne && dashCount < maxDashes) {
            controller.transform.position = transform.position + new Vector3(directionalInput.x * maxAirDashDist * (Time.deltaTime * 15), directionalInput.y * airDashSpeed * (Time.deltaTime), 0);
            dashCount++; 
		}
        if (directionalInput.x == -1 && airBorne && dashCount < maxDashes)
        {
           controller.transform.position = transform.position + new Vector3(directionalInput.x * maxAirDashDist * (Time.deltaTime * 15), directionalInput.y * airDashSpeed * (Time.deltaTime), 0);
            dashCount++;
        }
        if (directionalInput.x == 0) { dashCount = 0; }
    } 

	public void OnDashInputUp ()
	{
		

	}

	


	public void OnJumpInputDown ()
	{

		// Currently having problems with wall hops and wall climbs off of left walls. No clue why.  
		// Happened after adding PlayerInput script and adding OnInputDown/OnInputUp methods.  
		// FIXED IT AS OF 3/20/18 took PlayerInput script and put it back into player script.  fixed the issue. 



		if (wallCling & !controller.collisions.below) {
		
			velocity.x = -wallDirectionX * WallJump.x;
            velocity.y = maxJumpVelocity;
        }
		if (controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y; 
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x; 

				}
			} else {
				velocity.y = maxJumpVelocity;
				}
			}
		}

	public void OnJumpInputUp ()
	{
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
		}
	}


	void HandleDashing ()
	{
		airBorne = false; 
		if ((controller.collisions.left && controller.collisions.right) && controller.collisions.below) {
			airBorne = false; 
		}
		if ((!controller.collisions.left && !controller.collisions.right) && !controller.collisions.below) {
			airBorne = true; 
		}
		if (airBorne == false) {
			dashCount = 0; 
		}

	}

	void HandleWallCling ()
	{

		wallDirectionX = (controller.collisions.left) ? -1 : 1; 
		wallCling = false; 
		if ((controller.collisions.left) || (controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallCling = true; 
			if (velocity.y < -wallClingSpeedMax) {
				velocity.y = -wallClingSpeedMax; 
			}
			if (timeToWallUncling > 0) {

				velocity.x = 0; 
				velocityXSmoothing = 0; 

				
				if (directionalInput.x != wallDirectionX && directionalInput.x != 0) {
					timeToWallUncling -= Time.deltaTime; 
				} else {
					timeToWallUncling = wallClingTime; 
					}
				}
	     		  else  {
					timeToWallUncling = wallClingTime; 
			}

		}

	}

	void CalculateVelocity ()
	{
		float targetVelocityX = directionalInput.x * movespeed; 
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborn); 
		velocity.y += gravity * Time.deltaTime;

		// As far as I know, none of this is needed for air dashing :/ 
		//float targetAirVelocityX = directionalInput.x * maxAirDashDist;
		//airDashVelocity.x = Mathf.SmoothDamp (airDashVelocity.x, targetAirVelocityX, ref dashVelocitySmooth, airDashSpeed);
		//airDashVelocity.y = (gravity * Time.deltaTime) / 25; 
	}

}
