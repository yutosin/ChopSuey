using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyType
{
	BLACK_FLY,
	RED_FLY,
	BLUE_FLY,
	YELLOW_FLY,
	BEE,
	NONE
}

public class FlyController : MonoBehaviour
{
	[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private bool _drawCurve = true;

	public FlyType flyType;

	private HermiteMover _mover;
	
	void Start () 
	{
		transform.position = new Vector3(-HermiteMover.ScreenWidth * 2, 0, 0);
		if (_lineRenderer != null)
			_lineRenderer.useWorldSpace = true;

		_mover = GetComponent<HermiteMover>();
		
		if (!_mover)
			_mover = gameObject.AddComponent<HermiteMover>() as HermiteMover;
	}

	void Update () 
	{	
		if (_mover.FinishedRoute)
			Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("HitZone"))
		{
			if (flyType == FlyType.BEE)
				GameController.SharedInstance.UpdateScore(10, flyType);
			else
				GameController.SharedInstance.UpdateScore(1, flyType);
			Destroy(this.gameObject);
		}
	}
}
