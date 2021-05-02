using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public GameObject pausePanel, levelSwitchPanel, objectivePanel, instructionsPanel, menuElements;
	public Text levelSwitchText;
	public Text scoreText, blackScoreText, blueScoreText, redScoreText, beeScoreText;
	public Button nextLevelButton;
	[HideInInspector]public bool isPaused;

	private void Awake()
	{
		isPaused = false;
		Pause(true, false);
	}

	private void Start()
	{
		GameController.SharedInstance.OnScoreUpdate += UpdateScoreText;
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

	public void UpdateScoreText(FlyType type, int score)
	{
		if (GameController.SharedInstance.winCondition.winType == WinType.BUG_COLOR) 
		{
		 	BugColorWinCondition bugColorWinCondition = (BugColorWinCondition) GameController.SharedInstance.winCondition;
			switch (type)
			{
				case FlyType.BLACK_FLY:
					blackScoreText.text = "x" + score + "\n/" + bugColorWinCondition.blackFlyCount;
					break;
				case FlyType.BLUE_FLY:
					blueScoreText.text = "x" + score + "\n/" + bugColorWinCondition.blueFlyCount;
					break;
				case FlyType.RED_FLY:
					redScoreText.text = "x" + score + "\n/" + bugColorWinCondition.redFlyCount;
					break;
			}
		}
		else if (type == FlyType.NONE)
		{
			scoreText.text = "Score: " + score;
		}

		if (GameController.SharedInstance.winCondition.winType == WinType.BEE_PREVENT && type == FlyType.BEE)
		{
			BeePreventWinCondition beePreventWinCondition = 
				(BeePreventWinCondition) GameController.SharedInstance.winCondition;
			beeScoreText.text = "x" + score + "\n/" + beePreventWinCondition.beeCount;
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
