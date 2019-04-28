using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HermitePoints
{
	public Vector3 startPoint, endPoint, startTangentPoint, endTangentPoint;

	public HermitePoints(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1)
	{
		startPoint = p0;
		endPoint = p1;
		startTangentPoint = m0 - p0;
		endTangentPoint = m1 - p1;
	}
	
}

public class HermiteCurve
{
	private List<Vector3> _curvePoints;
	private HermitePoints _controlPoints;
	
	public Color lineColor = Color.cyan;
	public float lineWidth = 0.2f;

	public HermitePoints ControlPoints
	{
		get { return _controlPoints; }
	}

	public List<Vector3> CurvePoints
	{
		get { return _curvePoints; }
	}

	public HermiteCurve(HermitePoints controlPoints)
	{
		_curvePoints = new List<Vector3>(20);
		_controlPoints = controlPoints;
		
		float t;
		Vector3 position;

		for(int i = 0; i < _curvePoints.Capacity; i++)
		{
			t = i / (_curvePoints.Capacity - 1.0f);
			position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * controlPoints.startPoint 
			           + (t * t * t - 2.0f * t * t + t) * controlPoints.startTangentPoint
			           + (-2.0f * t * t * t + 3.0f * t * t) * controlPoints.endPoint
			           + (t * t * t - t * t) * controlPoints.endTangentPoint;
			_curvePoints.Add(position);
			//lineRenderer.SetPosition(i, position);
		}
	}

	public void DrawHermiteCurve(LineRenderer lineRenderer)
	{
		lineRenderer.startColor = lineColor;
		lineRenderer.endColor = lineColor;
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;
		if (_curvePoints.Count > 0)
		{
			lineRenderer.positionCount = _curvePoints.Count;
		}
		
		for (int i = 0; i < _curvePoints.Count; i++)
		{
			lineRenderer.SetPosition(i, _curvePoints[i]);
		}
	}

	public void DrawMultipleHermiteCurves(LineRenderer lineRenderer, List<HermiteCurve> curves)
	{
		lineRenderer.startColor = lineColor;
		lineRenderer.endColor = lineColor;
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;
		int numPoints = _curvePoints.Count * curves.Count;
		if (numPoints > 0)
		{
			lineRenderer.positionCount = numPoints;
		}

		for (int i = 0; i < curves.Count; i++)
		{
			for (int j = 0; j < _curvePoints.Count; j++)
			{
				lineRenderer.SetPosition(j + (i * 20), curves[i].CurvePoints[j]);
			}
		}
		
	}
}
