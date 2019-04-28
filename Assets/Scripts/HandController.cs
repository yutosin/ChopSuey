using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
	[SerializeField] private float rotateDuration = 0.1f;
	//[HideInInspector] public bool rotating = false;
	private Quaternion target = Quaternion.Euler(0, 50, 0);

	private float direction = 1.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
//		if (Input.GetKeyDown(KeyCode.Space))
//		{
//			StartCoroutine(Swat());
//		}
//		if (Input.GetKey(KeyCode.W))
//		{
//			
//			Move(1.0f);
//		}
//		else if (Input.GetKey(KeyCode.S))
//		{
//			Move(-1.0f);
//		}

		Vector3 pos = transform.position;
		pos.y += direction * 4.0f * Time.deltaTime;
		transform.position = pos;
		if (pos.y >= Camera.main.orthographicSize ||
		    pos.y <= -Camera.main.orthographicSize)
			direction = -direction;
	}

//	private void Move(float axis)
//	{
//		Vector3 pos = transform.position;
//		pos.y += axis * 4.0f * Time.deltaTime;
//		transform.position = pos;
//	}

//	private IEnumerator Swat()
//	{
//		if (rotating)
//			yield break;
//		rotating = true;
//		
//		Quaternion currentRot = transform.rotation;
//
//		float counter = 0;
//		while (counter < rotateDuration)
//		{
//			if (counter <= rotateDuration / 2)
//			{
//				counter += Time.deltaTime;
//				transform.rotation = Quaternion.Lerp(currentRot, target, counter / (rotateDuration / 2));
//				yield return null;
//			}
//			else
//			{
//				counter += Time.deltaTime;
//				transform.rotation = Quaternion.Lerp(currentRot, Quaternion.identity, counter / (rotateDuration / 2));
//				yield return null;
//			}
//		}
//
//		rotating = false;
//	}
}
