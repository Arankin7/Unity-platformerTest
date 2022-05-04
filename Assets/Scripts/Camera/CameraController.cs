using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target; 
	public float camY = .125f; 
	public float camRise = 0.3f;
	public float camFall = 0.6f;



	public Vector3 offSet; 

	public virtual void LateUpdate ()
	{
		 

		Vector3 desiredPos = target.transform.position + offSet;
		Vector3 smoothedPos = Vector3.Lerp (transform.position, desiredPos, camY); 
		transform.position = smoothedPos; 
		if (target.GetComponent<Player> ().velocity.y > Mathf.Abs (0)) {
			
			Vector3 risePos = Vector3.Lerp (transform.position, (target.transform.position + offSet), camRise);
			transform.position = risePos; 
			}
			if(target.GetComponent<Player>().velocity.y < Mathf.Abs(0)) {
			Vector3 fallPos = Vector3.Lerp (transform.position, (target.transform.position+offSet), camFall);
			transform.position = fallPos; 
			}


	} 
}