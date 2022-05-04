using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingOpposite : MonoBehaviour {

	public GameObject Blinker; 
	public float blinkTime = 1.5f; 
	public float goneTimer = 1f; 

	void Start () {
		StartCoroutine("PlatBlink"); 
		
	}
	public IEnumerator PlatBlink ()
	{		
			
		while (Blinker.activeInHierarchy) {

			Blinker.SetActive(false); 
			yield return new WaitForSeconds (goneTimer); 
			Blinker.SetActive(true); 
			yield return new WaitForSeconds (blinkTime); 
			Blinker.SetActive (false); 
			yield return new WaitForSeconds (goneTimer); 
			Debug.Log ("Blinking"); 
			Blinker.SetActive (true); 
			yield return new WaitForSeconds (blinkTime); 
		}
	}
}
