using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollectibleCount : MonoBehaviour {


	public int collectibles = 25;

	//private static int updatedCollectible; 

	public void OnTriggerEnter2D (Collider2D Other)
	{

		if (Other.gameObject.tag == ("Collectible")) {
			  Collect(); 
		} 

	}

	public void Collect ()
	{
			collectibles--; 
			print ("Collectibles = " + collectibles);
			//updatedCollectible = collectibles; 
			//print ("Updated Collectibles = " + updatedCollectible);
	}

	public void ResetCollectible (bool dead)
	{
		if (dead ==true) {
			collectibles = 0 ; 
			
		}
			
	}
}
