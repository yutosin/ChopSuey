using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreType
{
	TOTAL,
	COLOR
}

public class GameController : MonoBehaviour
{
	private static GameController _sharedInstance;
	private float timeStart, timeElapsed;
	private bool levelFinished = false;
	private ScoreType _scoreType;

	public delegate void ScoreUpdate(FlyType type, int score);

	public ScoreUpdate OnScoreUpdate;
	
	public WinCondition winCondition;
	
	public GameObject[] flyPrefabs;

	public GameObject beePrefab;

	public UIController uiController;

	public int levelLength = 120;

	public int score, blackScore, blueScore, redScore, yellowScore, beeScore  = 0;

	public Text scoreText, blackScoreText, blueScoreText, redScoreText, beeScoreText, timerText;

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
		_scoreType = ScoreType.TOTAL;
		StartCoroutine(SpawnFlies());
		StartCoroutine(SpawnYellowFlies());
		if (winCondition.winType == WinType.BEE_PREVENT)
			StartCoroutine(SpawnBees());
		if (winCondition.winType == WinType.BUG_COLOR)
			_scoreType = ScoreType.COLOR; 
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
			{
				SubscribeToFlyKilledEvent(
					Instantiate(beePrefab, Vector3.zero, Quaternion.identity));
			}

			yield return new WaitForSeconds(5f);
		}
	}
	
	private IEnumerator SpawnYellowFlies()
	{
		while (true)
		{
			int spawnChance = Random.Range(0, 2);
			if (spawnChance == 1)
			{
				SubscribeToFlyKilledEvent(
					Instantiate(flyPrefabs[3], Vector3.zero, Quaternion.identity));
			}

			yield return new WaitForSeconds(2f);
		}
	}

	private IEnumerator SpawnFlies()
	{
		while (true)
		{
			if (winCondition.winType == WinType.BUG_COLOR)
			{
				SubscribeToFlyKilledEvent(
					Instantiate(flyPrefabs[Random.Range(0, 4)], Vector3.zero, Quaternion.identity));
			}
			else
			{
				SubscribeToFlyKilledEvent(
					Instantiate(flyPrefabs[0], Vector3.zero, Quaternion.identity));
			}
			yield return new WaitForSeconds(.4f);
		}
	}

	private void SubscribeToFlyKilledEvent(GameObject fly)
	{
		FlyController flyController = fly.GetComponent<FlyController>();
		if (!flyController)
			return;
		flyController.FlyKilledEvent += UpdateScore;
	}

	public void UpdateTimeLeft()
	{
		timerText.text = "Seconds Left: " + (int)(levelLength - timeElapsed);
	}
	
	//i actually don't think how score is applied has to depend on win condition...certain flies only appear in certain
	//game modes anyway, so just always listen for score update events
	public void UpdateScore(FlyType flyType, int pointVal)
	{
		switch (flyType)
		{
			case FlyType.BLACK_FLY:
				blackScore += pointVal;
				break;
			case FlyType.BLUE_FLY:
				blueScore += pointVal;
				break;
			case FlyType.RED_FLY:
				redScore += pointVal;
				break;
			case FlyType.YELLOW_FLY:
				yellowScore -= pointVal;
				break;
			case FlyType.BEE:
				beeScore += pointVal;
				break;
		}
		
		if (OnScoreUpdate == null)
			return;

		if (_scoreType == ScoreType.TOTAL)
		{
			int total = blackScore + blueScore + redScore + yellowScore + beeScore;
			OnScoreUpdate(FlyType.NONE, total);
			if (beeScore > 0)
				OnScoreUpdate(FlyType.BEE, beeScore);
			return;
		}
		
		switch (flyType)
		{
			case FlyType.BLACK_FLY:
				OnScoreUpdate(flyType, blackScore);
				break;
			case FlyType.BLUE_FLY:
				OnScoreUpdate(flyType, blueScore);
				break;
			case FlyType.RED_FLY:
				OnScoreUpdate(flyType, redScore);
				break;
		}
	}
}
