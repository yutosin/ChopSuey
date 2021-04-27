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

//	public GameObject start, startTangentPoint, end, endTangentPoint;
//
//	public Color color = Color.white;
//	public float width = 0.2f;
//	public int numberOfPoints = 20;
	[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private bool _drawCurve = true;

	public FlyType flyType;

	private List<HermiteCurve> _flyPath;
	private int _currentTargetCurvePoint = 1;
	private int _currentCurve = 0;
	
	private float _screenWidth;
	private float _screenHeight;

	public GameObject controlPrefab, tanPrefab;
	
	void Start () 
	{
		_screenWidth =  Camera.main.orthographicSize * Camera.main.aspect;
		_screenHeight = Camera.main.orthographicSize;
		if (_lineRenderer != null)
		{
			_lineRenderer.useWorldSpace = true;
			//_lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		}

		_flyPath = GenerateCurves(4);
		transform.position = _flyPath[0].ControlPoints.startPoint;

	}

	protected virtual bool BoundCheck()
	{
		if (_currentCurve == _flyPath.Count || _currentTargetCurvePoint == _flyPath[_currentCurve].CurvePoints.Count)
		{
			Destroy(this.gameObject);
			return true;
		}

		return false;
	}
	
	void Update () 
	{	

		if (_drawCurve)
		{
			_flyPath[0].DrawMultipleHermiteCurves(_lineRenderer, _flyPath);
		}

		if(BoundCheck())
			return;
		
		Vector3 normal = (_flyPath[_currentCurve].CurvePoints[_currentTargetCurvePoint] - transform.position).normalized;
		transform.position = Vector3.MoveTowards(transform.position, transform.position + normal, Time.deltaTime * 10.0f);
		float d = Vector3.Distance(transform.position, _flyPath[_currentCurve].CurvePoints[_currentTargetCurvePoint]);
		if(d <= .1f
		   && _currentTargetCurvePoint <= _flyPath[_currentCurve].CurvePoints.Count - 1)
		{
			_currentTargetCurvePoint++;
		}

		if (_currentTargetCurvePoint == _flyPath[_currentCurve].CurvePoints.Count && _currentCurve <= _flyPath.Count)
		{
			_currentTargetCurvePoint = 0;
			_currentCurve++;
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("HitZone"))
		{
			GameController.SharedInstance.UpdateScore(1, flyType);
			Destroy(this.gameObject);
		}
		if (other.gameObject.CompareTag("Hand"))
		{
			GenerateCurves(4);
			_currentTargetCurvePoint = 0;
			_currentCurve = 0;

		}
	}
	
	//hard coded/repeated values in this function, clean that up; might be better approach to handle initial curve point
	//case as well
	private List<HermiteCurve> GenerateCurves(int numPoints)
	{
		//define increments for control point placement along x axis; should be evenly spaced across screen
		//note: 4 is a standard x-offset value, don't hard code
		float xStandardIncrement = (_screenWidth * 2 + 4) / (numPoints - 1);
		float xInit = -_screenWidth - 4;
		
		List<HermiteCurve> hermiteCurves = new List<HermiteCurve>(numPoints - 1);
		
		for (int i = 0; i < hermiteCurves.Capacity; i++)
		{
			float xIncrement;
			//value is used so that the next curve can either progress forward or backwards along x-axis, also to add
			//variation along x axis points (not every fly hits the same curve shift point)
			int xWiggle;
			float y;
			float tanX;
			float tanY;
			Vector3 p0, p1, m0, m1; //start point, end point, start tangent, end tandgent
			
			//i == 0 case is for the first point, off screen; since it has no prior curve as reference for the start point
			//it must generate its own
			if (i == 0)
			{
				xIncrement = i * xStandardIncrement;
				xWiggle = Random.Range(-2, 3);
				y = Random.Range(-_screenHeight - 1, _screenHeight);
				p0 = new Vector3(xInit + xIncrement + xWiggle, y, 0);
				
				tanX = Random.Range(p0.x + 1, _screenWidth + 4);
				tanY = Random.Range(-_screenHeight - 1, _screenHeight);
				m0 = new Vector3(tanX, tanY, 0);
			}
			else
			{
				//set to 0 since the start point and start tangent point are sourced from previous curve end info
				p0 = new Vector3(0, 0, 0);
				m0 = new Vector3(0, 0, 0);
			}
			
			//calculate info for end point of curve
			xIncrement = (i + 1) * xStandardIncrement;
			xWiggle = Random.Range(-2, 3);
			y = Random.Range(-_screenHeight - 1, _screenHeight);
			p1 = new Vector3(xInit + xIncrement + xWiggle, y, 0);

			tanX = Random.Range(p1.x + 1, _screenWidth + 4);
			tanY = Random.Range(-_screenHeight - 1, _screenHeight);
			m1 = new Vector3(tanX, tanY, 0);
			
			HermitePoints controlPoints = new HermitePoints(p0, p1, m0, m1);
			//to create a spline (connected curves) tangent at the end point of one Hermite curve is the same as the
			//tangent of the start point of the next Hermite curve
			if (i > 0)
			{
				controlPoints.startPoint = hermiteCurves[i - 1].ControlPoints.endPoint;
				controlPoints.startTangentPoint = hermiteCurves[i - 1].ControlPoints.endTangentPoint;
			}
			
			hermiteCurves.Add(new HermiteCurve(controlPoints));
		}

		return hermiteCurves;
	}
}
