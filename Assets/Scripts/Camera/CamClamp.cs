using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamClamp : MonoBehaviour {

	[SerializeField] Transform target; 

	 

	public float yMaxValue = 1; 
	public float yMinValue = -1; 
	public float xMaxValue = 1; 
	public float xMinValue = -1; 

	public bool yMaxEnabled = false; 
	public bool yMinEnabled = false; 
	public bool xMaxEnabled = false; 
	public bool xMinEnabled = false; 

	Transform targetPos;

	void Awake ()
	{
		targetPos = transform; 
	}

	void LateUpdate ()
	{

		float X = Mathf.Clamp(target.position.x, xMinValue, xMaxValue);
		float y = Mathf.Clamp(target.position.y, yMinValue, yMaxValue); 
		targetPos.position = new Vector3 (X, y, targetPos.position.z); 
		
		
	}
}
