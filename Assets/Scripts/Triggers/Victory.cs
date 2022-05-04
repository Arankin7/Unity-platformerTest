using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (Rigidbody2D))]
public class Victory : MonoBehaviour {
	public Transform player; 

		public void OnTriggerEnter2D (Collider2D other)
	{
		//Debug.Log ("Victory"); 
	
		if (other.CompareTag ("Player")) {

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
