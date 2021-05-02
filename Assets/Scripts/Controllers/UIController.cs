using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public GameObject pausePanel, levelSwitchPanel, objectivePanel, instructionsPanel, menuElements;
	public Text levelSwitchText;
	public Button nextLevelButton;
	[HideInInspector]public bool isPaused;

	private void Awake()
	{
		isPaused = false;
		Pause(true, false);
	}

	public void OnStartButtonClick()
	{
		Pause(true, false);
		SceneManager.LoadScene("_Scene_1");
	}

	public void OnBeginButtonClick()
	{
		Pause(false, false);
		objectivePanel.SetActive(false);
	}

	public void OnInstructionsButtonClick()
	{
		menuElements.SetActive(false);
		instructionsPanel.SetActive(true);
	}
	
	public void OnQuitButtonClick()
	{
		Application.Quit();
	}

	public void OnResumeButtonClick()
	{
		Pause(false, true);
	}

	public void OnRetryButtonClick()
	{
		Pause(false, false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnNextLevelButtonClick(string level)
	{
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
			if (isPaused)
				return;
			if (pausePanel != null && togglePausePanel && !pausePanel.activeInHierarchy)
				pausePanel.SetActive(true);
			Time.timeScale = 0f;
			isPaused = true;
		}
		else
		{
			if (!isPaused)
				return;
			if (pausePanel != null && togglePausePanel && pausePanel.activeInHierarchy)
				pausePanel.SetActive(false);
			Time.timeScale = 1f;
			isPaused = false;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !isPaused && SceneManager.GetActiveScene().name != "MainMenu")
		{
			Pause(true, true);
		}
	}
}
