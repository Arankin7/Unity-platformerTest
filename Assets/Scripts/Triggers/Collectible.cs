using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	 
	//public int collectibles = 25;


	void OnTriggerEnter2D (Collider2D Other)
	{

		if (Other.gameObject.tag == ("Player")) {
		 	 
			//gameObject.SetActive(false); 
			Destroy(gameObject); 
		}
		
	}
}
