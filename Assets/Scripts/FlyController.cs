using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {

//	public GameObject start, startTangentPoint, end, endTangentPoint;
//
//	public Color color = Color.white;
//	public float width = 0.2f;
//	public int numberOfPoints = 20;
	[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private bool _drawCurve = true;

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
			_lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		}

		_flyPath = GenerateCurves(4);
		transform.position = _flyPath[0].ControlPoints.startPoint;

	}
	
	void Update () 
	{
//		// check parameters and components
//		if (null == lineRenderer || null == start || null == startTangentPoint 
//		    || null == end || null == endTangentPoint)
//		{
//			return; // no points specified
//		} 
//
//		// update line renderer
//		lineRenderer.startColor = color;
//		lineRenderer.endColor = color;
//		lineRenderer.startWidth = width;
//		lineRenderer.endWidth = width;
//		if (numberOfPoints > 0)
//		{
//			lineRenderer.positionCount = numberOfPoints;
//		}
//
//		// set points of Hermite curve
//		Vector3 p0 = start.transform.position;
//		Vector3 p1 = end.transform.position;
//		Vector3 m0 = startTangentPoint.transform.position - start.transform.position;
//		Vector3 m1 = endTangentPoint.transform.position - end.transform.position;
//		float t;
//		Vector3 position;
//
//		for(int i = 0; i < numberOfPoints; i++)
//		{
//			t = i / (numberOfPoints - 1.0f);
//			position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0 
//			           + (t * t * t - 2.0f * t * t + t) * m0 
//			           + (-2.0f * t * t * t + 3.0f * t * t) * p1 
//			           + (t * t * t - t * t) * m1;
//			lineRenderer.SetPosition(i, position);
//		}
		

		if (_drawCurve)
		{
//			foreach (HermiteCurve curve in _flyPath)
//			{
//				curve.DrawHermiteCurve(_lineRenderer);
//			}
			_flyPath[0].DrawMultipleHermiteCurves(_lineRenderer, _flyPath);
		}

		if (_currentCurve == _flyPath.Count || _currentTargetCurvePoint == _flyPath[_currentCurve].CurvePoints.Count)
		{
			Destroy(this.gameObject);
			return;
		}

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
			GameController.SharedInstance.UpdateScore(1);
			Destroy(this.gameObject);
		}
		if (other.gameObject.CompareTag("Hand"))
		{
//			HandController hand = other.gameObject.transform.parent.gameObject.GetComponent<HandController>();
//			if (!hand.rotating)
//				return;
			GenerateCurves(4);
			_currentTargetCurvePoint = 0;
			_currentCurve = 0;

		}
	}

	private List<HermiteCurve> GenerateCurves(int numPoints)
	{
		float xStandardIncrement = (_screenWidth * 2 + 4) / (numPoints - 1);
		float xInit = -_screenWidth - 4;
		
		List<HermiteCurve> hermiteCurves = new List<HermiteCurve>(numPoints - 1);
		
		for (int i = 0; i < hermiteCurves.Capacity; i++)
		{
			float xIncrement;
			float xWiggle;
			float y;
			float tanX;
			float tanY;
			Vector3 p0, p1, m0, m1;

			if (i == 0)
			{
				xIncrement = i * xStandardIncrement;
				xWiggle = Random.Range(-2.0f, 3.0f);
				y = Random.Range(-_screenHeight - 1, _screenHeight);
				p0 = new Vector3(xInit + xIncrement + xWiggle, y, 0);
				
				tanX = Random.Range(p0.x + 1, _screenWidth + 4);
				tanY = Random.Range(-_screenHeight - 1, _screenHeight);
				m0 = new Vector3(tanX, tanY, 0);
			}
			else
			{
				p0 = new Vector3(0, 0, 0);
				m0 = new Vector3(0, 0, 0);
			}
			
			xIncrement = (i + 1) * xStandardIncrement;
			xWiggle = Random.Range(-2.0f, 3.0f);
			y = Random.Range(-_screenHeight - 1, _screenHeight);
			p1 = new Vector3(xInit + xIncrement + xWiggle, y, 0);

			tanX = Random.Range(p1.x + 1, _screenWidth + 4);
			tanY = Random.Range(-_screenHeight - 1, _screenHeight);
			m1 = new Vector3(tanX, tanY, 0);
			
			HermitePoints controlPoints = new HermitePoints(p0, p1, m0, m1);
			if (i > 0)
			{
				controlPoints.startPoint = hermiteCurves[i - 1].ControlPoints.endPoint;
				controlPoints.startTangentPoint = hermiteCurves[i - 1].ControlPoints.endTangentPoint;
			}

//			Instantiate(controlPrefab, controlPoints.startPoint, Quaternion.identity);
//			Instantiate(tanPrefab, m0, Quaternion.identity);
//			Instantiate(controlPrefab, controlPoints.endPoint, Quaternion.identity);
//			Instantiate(tanPrefab, m1, Quaternion.identity);
			
			hermiteCurves.Add(new HermiteCurve(controlPoints));
		}

		return hermiteCurves;
	}
}
