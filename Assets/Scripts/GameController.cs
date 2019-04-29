using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	private static GameController _sharedInstance;
	
	public GameObject flyPrefab;

	public int score = 0;

	public Text scoreText;

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
		StartCoroutine(spawnFlies());
		UpdateScore(0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private IEnumerator spawnFlies()
	{
		while (true)
		{
			Instantiate(flyPrefab, Vector3.zero, Quaternion.identity);
			yield return new WaitForSeconds(.4f);
		}
	}

	public void UpdateScore(int pointVal)
	{
		score += pointVal;
		scoreText.text = "Score: " + score;
	}
}
