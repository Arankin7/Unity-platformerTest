using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour {


	public Texture2D fadeOut; 
	public float fadeSpeed = 1f; 

	private int drawDepth = 1000; // the textures draw order in the heirarchy, lower number means it renders on top. 
	private float alpha = 1f; 
	private int fadeDir = -1;  
	//float fadeTime = GameObject.Find("EmptyFadeObject").GetComponent<Fade>().BeginFade(1); 

	void OnGUI ()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime; 

		alpha = Mathf.Clamp01(alpha); 

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha); 
		GUI.depth = drawDepth;  // make sure black color renders on top 
		GUI.DrawTexture  (new Rect (0, 0, Screen.width, Screen.height), fadeOut); 
	} 

	public float BeginFade (int direction)
	{
		fadeDir = direction; 
		return (fadeSpeed); 
		//yield return new WaitForSeconds (fadeTime); 

	} 

	public void SceneLoaded ()
	{ 
		BeginFade (-1); 
	}
}
