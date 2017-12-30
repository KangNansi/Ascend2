using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class CurveTest : MonoBehaviour {

    BGCcMath math;
    float time = 0;

	// Use this for initialization
	void Start () {
        math = GetComponent<BGCcMath>();
	}
	
	// Update is called once per frame
	void Update () {
        time = (time + Time.deltaTime) % 1f;
        Debug.DrawLine(math.CalcByDistanceRatio(BGCurveBaseMath.Field.Position, time), Vector3.zero);
	}
}
