using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Timer : MonoBehaviour {

	Text timerText; 

	float timerStartTime; 


	public void Start ()
	{
		timerText = GetComponent<Text>(); 
		timerStartTime = Time.timeSinceLevelLoad; 

	}

	void Update ()
	{

		float t = Time.timeSinceLevelLoad - timerStartTime; 

		 string timerMinutes = ((int) t / 60).ToString(); 
		 string timerSeconds = (t % 60).ToString ("F2"); 

		  

		timerText.text =  timerMinutes + ":" + timerSeconds;
		//string.Format ("{1:00}:{2:00}", timerMinutes,timerSeconds);   



		  
	} 

	
}
