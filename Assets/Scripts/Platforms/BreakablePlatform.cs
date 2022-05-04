using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
[RequireComponent(typeof (Rigidbody2D))]

public class BreakablePlatform : MonoBehaviour {

	public float platWait = .5f; 


	// Player freezes in UNITYPLAYER on collision of Breakable Object. 
	// Fixed on 4/2/18 - Deleted all old Builds of game and restarted Computer.  	

	public IEnumerator BreakPlat ()
	{  
			yield return new WaitForSeconds(platWait);
			gameObject.SetActive(false); 
    }

   public void OnTriggerEnter2D (Collider2D other)
	{
		
		if (other.gameObject.tag == ("Player")) {
			StartCoroutine ("BreakPlat");
		}

	}
}
