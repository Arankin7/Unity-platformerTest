using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to an Empty Game Object with a Platform as a Child.  Or just a game object in the scene. 

public class BlinkingPlatform : MonoBehaviour {

	public GameObject Blinker; 
	public float blinkTime = 1.5f; 
	public float goneTimer = 1f; 

	void Start () {

		StartCoroutine("PlatBlink"); 
		
	}
	public IEnumerator PlatBlink ()
	{		

		
		while (Blinker.activeInHierarchy) {
			yield return new WaitForSeconds (blinkTime); 
			Blinker.SetActive (false); 
			yield return new WaitForSeconds (goneTimer); 
			Debug.Log ("Blinking"); 
			Blinker.SetActive (true); 
		}
	}
}
