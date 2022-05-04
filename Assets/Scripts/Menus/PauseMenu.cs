using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour {

// Make sure PauseMenu/Canvas Has an eventsystem with it!!!!

	public static bool GameIsPaused = false; 
	public GameObject pauseMenuUI; 


	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.P)) {
			if (GameIsPaused)
			{
				Resume ();
			} else {
				Pause ();
			}
		}
		
	}
	public void Resume ()
	{
		pauseMenuUI.SetActive(false); 
		Time.timeScale = 1f; 
		GameIsPaused = false; 
	}

	void Pause ()
	{
		pauseMenuUI.SetActive(true); 
		Time.timeScale = 0f; 
		GameIsPaused = true; 

	}

	public void LoadMenu ()
	{
		Time.timeScale = 1f; 
		SceneManager.LoadScene("_MainMenu");
	}

	public void Restart ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1f; 

	}

	public void LevelSelect ()
	{
		//Takes you to level select screen when that is set up! 
	}
}
