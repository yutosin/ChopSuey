using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct HermitePoints_new
{
    public Vector3 startPoint, endPoint, startTangentPoint, endTangentPoint;

    public HermitePoints_new(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1)
    {
        startPoint = p0;
        endPoint = p1;
        startTangentPoint = m0 - p0;
        endTangentPoint = m1 - p1;
    }
	
}

public class HermiteMover : MonoBehaviour
{
    private List<HermitePoints_new> _curveControlPoints;
    private int _numCurves;
    private int _currentCurve;
    private float _tParam;

    [HideInInspector]public bool FinishedRoute;
    private bool _coroutineAllowed;

    public static float ScreenWidth, ScreenHeight;

    private void Start()
    {
	    _curveControlPoints = new List<HermitePoints_new>(4);
	    _numCurves = 3;
	    _currentCurve = 0;
	    FinishedRoute = false;
	    _tParam = 0f;
	    _coroutineAllowed = true;
	    
	    ScreenWidth =  Camera.main.orthographicSize * Camera.main.aspect;
	    ScreenHeight = Camera.main.orthographicSize;
		
	    GenerateHermitePoints();
    }

    private void Update()
    {
	    if (_coroutineAllowed)
		    StartCoroutine(MoveAlongHermiteCurve());
    }

    private IEnumerator MoveAlongHermiteCurve()
    {
	    _coroutineAllowed = false;
	    
	    while (_tParam < 1)
	    {
		    _tParam += Time.deltaTime * 0.5f;
		    
		    Vector3 curvePoint = (2.0f *_tParam*_tParam*_tParam- 3.0f *_tParam*_tParam+ 1.0f) * _curveControlPoints[_currentCurve].startPoint 
		                         + (_tParam *_tParam*_tParam- 2.0f *_tParam*_tParam+ _tParam) * _curveControlPoints[_currentCurve].startTangentPoint
		                         + (-2.0f *_tParam*_tParam*_tParam+ 3.0f *_tParam* _tParam) * _curveControlPoints[_currentCurve].endPoint
		                         + (_tParam *_tParam*_tParam-_tParam* _tParam) * _curveControlPoints[_currentCurve].endTangentPoint;

		    transform.position = curvePoint;

		    yield return new WaitForEndOfFrame();
	    }

	    _tParam = 0f;
	    
	    _currentCurve++;

	    if (_currentCurve > _numCurves)
	    {
		    FinishedRoute = true;
		    _coroutineAllowed = false;

		    yield break;
	    }

	    _coroutineAllowed = true;
    }
    
    public void GenerateHermitePoints()
    {
        float xStandardIncrement = (ScreenWidth * 2 + 4) / _numCurves;
        float xInit = -ScreenWidth - 4;
        
        for (int i = 0; i < _numCurves + 1; i++)
        {
	        float xIncrement;
	        //value is used so that the next curve can either progress forward or backwards along x-axis, also to add
	        //variation along x axis points (not every fly hits the same curve shift point)
	        int xWiggle;
	        float y;
	        float tanX;
	        float tanY;
	        Vector3 p0, p1, m0, m1; //start point, end point, start tangent, end tangent
        			
	        //i == 0 case is for the first point, off screen; since it has no prior curve as reference for the start point
	        //it must generate its own
	        if (i == 0)
	        {
		        xIncrement = i * xStandardIncrement;
		        xWiggle = Random.Range(-2, 3);
		        y = Random.Range(-ScreenHeight - 1, ScreenHeight);
		        p0 = new Vector3(xInit + xIncrement + xWiggle, y, 0);
        				
		        tanX = Random.Range(p0.x + 1, ScreenWidth + 4);
		        tanY = Random.Range(-ScreenHeight - 1, ScreenHeight);
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
	        y = Random.Range(-ScreenHeight - 1, ScreenHeight);
	        p1 = new Vector3(xInit + xIncrement + xWiggle, y, 0);
        
	        tanX = Random.Range(p1.x + 1, ScreenWidth + 4);
	        tanY = Random.Range(-ScreenHeight - 1, ScreenHeight);
	        m1 = new Vector3(tanX, tanY, 0);
        			
	        HermitePoints_new controlPoints = new HermitePoints_new(p0, p1, m0, m1);
	        //to create a spline (connected curves) tangent at the end point of one Hermite curve is the same as the
	        //tangent of the start point of the next Hermite curve
	        if (i > 0)
	        {
		        controlPoints.startPoint = _curveControlPoints[i - 1].endPoint;
		        controlPoints.startTangentPoint = _curveControlPoints[i - 1].endTangentPoint;
	        }
	        _curveControlPoints.Add(controlPoints);
        }
    }
}
