using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
	public GameObject pausePanel;
	
	public void OnStartButtonClick()
	{
		SceneManager.LoadScene("_Scene_1");
	}

	public void OnInstructionsButtonClick()
	{
		
	}
	
	public void OnQuitButtonClick()
	{
		Application.Quit();
	}

	public void OnResumeButtonClick()
	{
		pausePanel.SetActive(false);
		Time.timeScale = 1f;
		GameController.SharedInstance.isPaused = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !GameController.SharedInstance.isPaused && SceneManager.GetActiveScene().name != "MainMenu")
		{
			pausePanel.SetActive(true);
			Time.timeScale = 0f;
			GameController.SharedInstance.isPaused = true;
		}
	}
}
