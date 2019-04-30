using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public GameObject pausePanel, levelSwitchPanel;
	public Text levelSwitchText;
	public Button nextLevelButton;
	
	public void OnStartButtonClick()
	{
		Pause(false, false);
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
		Pause(false, true);
		//		pausePanel.SetActive(false);
//		Time.timeScale = 1f;
//		GameController.SharedInstance.isPaused = false;
	}

	public void OnRetryButtonClick()
	{
		Pause(false, false);
//		Time.timeScale = 1f;
//		GameController.SharedInstance.isPaused = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnNextLevelButtonClick(string level)
	{
		Pause(false, true);
//		Time.timeScale = 1f;
//		GameController.SharedInstance.isPaused = false;
		SceneManager.LoadScene(level);
	}

	public void OnMainMenuButtonClick()
	{
		Pause(true, false);
		SceneManager.LoadScene("MainMenu");
	}

	public void SwitchLevel(bool win)
	{
		levelSwitchPanel.SetActive(true);
		Pause(true, false);
//		Time.timeScale = 0f;
//		GameController.SharedInstance.isPaused = true;

		if (win)
		{
			levelSwitchText.text = "You won!";
			nextLevelButton.interactable = true;
		}
		else
		{
			levelSwitchText.text = "You lost!";
		}
	}

	private void Pause(bool status, bool togglePausePanel)
	{
		if (status)
		{
			if (GameController.SharedInstance.isPaused)
				return;
			if (togglePausePanel && !pausePanel.activeInHierarchy)
				pausePanel.SetActive(true);
			Time.timeScale = 0f;
			GameController.SharedInstance.isPaused = true;
		}
		else
		{
			if (!GameController.SharedInstance.isPaused)
				return;
			if (togglePausePanel && pausePanel.activeInHierarchy)
				pausePanel.SetActive(false);
			Time.timeScale = 1f;
			GameController.SharedInstance.isPaused = false;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !GameController.SharedInstance.isPaused && SceneManager.GetActiveScene().name != "MainMenu")
		{
			Pause(true, true);
//			pausePanel.SetActive(true);
//			Time.timeScale = 0f;
//			GameController.SharedInstance.isPaused = true;
		}
	}
}
