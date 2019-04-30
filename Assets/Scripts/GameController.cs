using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	private static GameController _sharedInstance;
	private float timeStart, timeElapsed;
	private bool levelFinished = false;
	
	public WinCondition winCondition;
	
	public GameObject[] flyPrefabs;

	public GameObject beePrefab;

	public UIController uiController;

	public int levelLength = 120;

	public int score, blackScore, blueScore, redScore, beeScore  = 0;

	public Text scoreText, blackScoreText, blueScoreText, redScoreText, beeScoreText, timerText;

	public bool isPaused = false;
	
	public static GameController SharedInstance
	{
		get { return _sharedInstance; }
	}
	
	// Use this for initialization
	void Start ()
	{
		if (_sharedInstance != null)
		{
			Destroy(gameObject);
			return;
		}
		
		_sharedInstance = this;
		timeStart = Time.time;
		StartCoroutine(SpawnFlies());
		if (winCondition.winType == WinType.BEE_PREVENT)
			StartCoroutine(SpawnBees());
		UpdateScore(0, FlyType.NONE);
		if (winCondition.winType == WinType.BUG_COLOR)
		{
			UpdateScore(0, FlyType.BLACK_FLY);
			UpdateScore(0, FlyType.BLUE_FLY);
			UpdateScore(0, FlyType.RED_FLY);
		}
		if (winCondition.winType == WinType.BEE_PREVENT)
			UpdateScore(0, FlyType.BEE);
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeElapsed = Time.time - timeStart;
		UpdateTimeLeft();

		if (timeElapsed >= levelLength && !levelFinished)
		{
			if (winCondition.CheckWin())
			{
				uiController.SwitchLevel(true);
				Debug.Log("Win!");
			}
			else
			{
				uiController.SwitchLevel(false);
				Debug.Log("Lose!");
			}
			levelFinished = true;
		}
	}

	private IEnumerator SpawnBees()
	{
		while (true)
		{
			int spawnChance = Random.Range(0, 2);
			if (spawnChance == 1)
				Instantiate(beePrefab, Vector3.zero, Quaternion.identity);
			yield return new WaitForSeconds(5f);
		}
	}

	private IEnumerator SpawnFlies()
	{
		while (true)
		{
			if (winCondition.winType == WinType.BUG_COLOR)
				Instantiate(flyPrefabs[Random.Range(0, 3)], Vector3.zero, Quaternion.identity);
			else
				Instantiate(flyPrefabs[0], Vector3.zero, Quaternion.identity);
			yield return new WaitForSeconds(.4f);
		}
	}

	public void UpdateTimeLeft()
	{
		timerText.text = "Seconds Left: " + (int)(levelLength - timeElapsed);
	}

	public void UpdateScore(int pointVal, FlyType flyType)
	{
		if (winCondition.winType == WinType.BUG_COLOR)
		{
			BugColorWinCondition bugColorWinCondition = (BugColorWinCondition) winCondition;
			switch (flyType)
			{
				case FlyType.BLACK_FLY:
					blackScore += pointVal;
					blackScoreText.text = "x" + blackScore + "\n/" + bugColorWinCondition.blackFlyCount;
					break;
				case FlyType.BLUE_FLY:
					blueScore += pointVal;
					blueScoreText.text = "x" + blueScore + "\n/" + bugColorWinCondition.blueFlyCount;
					break;
				case FlyType.RED_FLY:
					redScore += pointVal;
					redScoreText.text = "x" + redScore + "\n/" + bugColorWinCondition.redFlyCount;
					break;
			}
		}
		else if (winCondition.winType == WinType.BEE_PREVENT)
		{
			BeePreventWinCondition beePreventWinCondition = (BeePreventWinCondition) winCondition;
			beeScore += pointVal;
			beeScoreText.text = "x" + beeScore + "\n/" + beePreventWinCondition.beeCount;
		}
		else
		{
			score += pointVal;
			scoreText.text = "Score: " + score;
		}
	}
}
