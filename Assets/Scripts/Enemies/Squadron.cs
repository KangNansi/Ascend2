using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class Squadron : MonoBehaviour {

    public BGCcMath path;
    float distance = 0;
    public float speed = 10f;
    bool running = false;

    // Use this for initialization
    void Start () {
        path.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
    }
	
	// Update is called once per frame
	void Update () {
        

        //Path
        if (running)
        {
            distance += Time.deltaTime * speed;
            Vector3 tangent;
            transform.position = path.CalcPositionAndTangentByDistance(distance, out tangent);
            transform.rotation = Quaternion.LookRotation(tangent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SpaceShip ship = other.gameObject.GetComponent<SpaceShip>();
        if (ship != null)
        {
            target = ship;
            running = true;
        }
    }
}
