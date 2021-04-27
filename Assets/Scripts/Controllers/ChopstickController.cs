using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopstickController : MonoBehaviour
{
	public GameObject topChopStick;
	public Collider captureSphere;
	public AudioSource clickSound;

	[SerializeField] private float rotateDuration;
	
	private Vector3 mousePosition;
	private float zPos = 10f;
	private bool rotating;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameController.SharedInstance.uiController.isPaused)
			return;
		Vector3 temp = Input.mousePosition;
		temp.z = zPos;
		mousePosition = Camera.main.ScreenToWorldPoint(temp);
		Vector3 pos = Camera.main.WorldToViewportPoint(mousePosition);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos.z = zPos;
		transform.position = Camera.main.ViewportToWorldPoint(pos);

		if (Input.GetMouseButtonDown(0))
			StartCoroutine(Click());

//		if (Input.GetMouseButton(0))
//		{
//			captureSphere.enabled = true;
//			Vector3 tempLocal = topChopStick.transform.localPosition;
//			tempLocal.x = -0.7f;
//			topChopStick.transform.localPosition = tempLocal;
//
//			topChopStick.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 25));
//			
//		}
//		else
//		{
//			captureSphere.enabled = false;
//			Vector3 tempLocal = topChopStick.transform.localPosition;
//			tempLocal.x = 0f;
//			topChopStick.transform.localPosition = tempLocal;
//
//			topChopStick.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
//		}
	}
	
	private IEnumerator Click()
	{
		if (rotating)
			yield break;
		rotating = true;
		
		clickSound.Play();
		
		Quaternion currentRot = topChopStick.transform.rotation;

		float counter = 0;
		while (counter < rotateDuration)
		{
			if (counter <= rotateDuration / 4)
			{
				captureSphere.enabled = true;
				Vector3 tempLocal = topChopStick.transform.localPosition;
				tempLocal.x = -0.7f;
				topChopStick.transform.localPosition = tempLocal;
				counter += Time.deltaTime;
				topChopStick.transform.localRotation = Quaternion.Lerp(currentRot, Quaternion.Euler(new Vector3(0, 0, -25)), 
					counter / (rotateDuration / 4));
				yield return null;
			}
			else
			{
				captureSphere.enabled = false;
				Vector3 tempLocal = topChopStick.transform.localPosition;
				tempLocal.x = 0f;
				topChopStick.transform.localPosition = tempLocal;
				counter += Time.deltaTime;
				topChopStick.transform.localRotation = Quaternion.Lerp(currentRot, Quaternion.Euler(new Vector3(0, 0, -42)), 
					counter / (rotateDuration / 2));
				yield return null;
			}
		}

		rotating = false;
	}
}
