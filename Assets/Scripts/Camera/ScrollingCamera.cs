using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCamera : MonoBehaviour {

	public Transform [] backgrounds; 
	private float [] parallaxScales; 
	public float smoothing = 1f; 

	private Transform camPos; 

	private Vector3 previousCamPos; 


	void Awake ()
	{
		camPos = Camera.main.transform; 
	}


	void Start ()
	{
		previousCamPos = camPos.position; 

		parallaxScales = new float[backgrounds.Length]; 

		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z*-1; 
		}
	}


	void Update ()
	{
		for (int i = 0; i < backgrounds.Length; i++) {
			float parallax = (previousCamPos.x - camPos.position.x) * parallaxScales[i];
			float backgroundTargetPosX = backgrounds[i].position.x + parallax; 
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z); 
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);   
		}

		previousCamPos = camPos.position; 


	}




}
