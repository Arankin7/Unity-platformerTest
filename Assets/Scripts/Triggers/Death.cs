using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

[RequireComponent (typeof (Rigidbody2D))] 
public class Death : MonoBehaviour {

	public Transform player; 


	// Death Object requires a Rigigbody component (Kinematic)

	public void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player")) {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

	}
}
