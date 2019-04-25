using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopstickController : MonoBehaviour
{
	public GameObject topChopStick;
	public Collider captureSphere;
	
	private Vector3 mousePosition;
	private float zPos = 10f;
	
	private float test;

	// Use this for initialization
	void Start ()
	{
		test =  Camera.main.orthographicSize * (Screen.width / Screen.height);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 temp = Input.mousePosition;
		temp.z = zPos;
		mousePosition = Camera.main.ScreenToWorldPoint(temp);
		Vector3 pos = Camera.main.WorldToViewportPoint(mousePosition);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos.z = zPos;
		transform.position = Camera.main.ViewportToWorldPoint(pos);

		if (Input.GetMouseButton(0))
		{
			captureSphere.enabled = true;
			Vector3 tempLocal = topChopStick.transform.localPosition;
			tempLocal.x = -0.7f;
			topChopStick.transform.localPosition = tempLocal;

			topChopStick.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 25));
			
		}
		else
		{
			captureSphere.enabled = false;
			Vector3 tempLocal = topChopStick.transform.localPosition;
			tempLocal.x = 0f;
			topChopStick.transform.localPosition = tempLocal;

			topChopStick.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
		}
	}
}
