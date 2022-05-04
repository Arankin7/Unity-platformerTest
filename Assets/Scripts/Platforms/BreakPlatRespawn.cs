using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatRespawn : MonoBehaviour {
	public GameObject BreakRespawn; 

	public float platRespawn = 2f;


	// Fixed issue where all Respawning platforms would respawn at the same time on 01/23/19
	// added "StartCoroutine" into the Controller2D script with vertical and horizontal collisions. 

	  
	public void Start ()
	{

			StartCoroutine("PlatRespawn"); 
		
	}
	IEnumerator PlatRespawn ()
	{		
		while (BreakRespawn.activeInHierarchy) {
			yield return new WaitForSeconds (platRespawn); 
			Debug.Log ("Respawning"); 
			BreakRespawn.SetActive (true); 
		}
	}


}			
