using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class Squadron : MonoBehaviour {

    public BGCcMath path;
    float distance = 0;
    public float CurrentDistance
    {
        get
        {
            return distance;
        }
    }
    float runDistance = 0f;
    public float speed = 10f;
    bool running = false;
    Vector3 offset;

    // Use this for initialization
    void Start () {
        path.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
    }
	
	// Update is called once per frame
	void Update () {
        //Path
        if (running)
        {
            runDistance += Time.deltaTime * speed;
            distance -= Time.deltaTime * speed;
            Vector3 tangent;
            Vector3 curvePosition = path.CalcPositionAndTangentByDistance(distance, out tangent);
            transform.position = curvePosition;
            transform.rotation = Quaternion.LookRotation(-tangent);
            transform.position += (transform.forward * offset.z + transform.right * offset.x + transform.up * offset.y);
            if(runDistance > 300f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SpaceShip ship = other.gameObject.GetComponent<SpaceShip>();
        if (ship != null)
        {
            running = true;
        }
    }

    public void Launch(BGCcMath curve, float dist = -1f, Vector3 off = new Vector3())
    {
        path = curve;
        if(dist>=0)
        {
            distance = dist;
        }
        offset = off;
        running = true;
    }
}
