using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject flyPrefab;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(spawnFlies());
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
}
