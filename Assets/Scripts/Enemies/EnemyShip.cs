using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class EnemyShip : Enemy {

    public Weapon weapon;
    public BGCcMath localCurve = null;
    public float curveSpeed = 10f;
    float distance = 0f;

    SpaceShip target = null;




	// Use this for initialization
	new void Start () {
        base.Start();
        if(localCurve != null)
        {
            localCurve.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
            /*localCurve.SetParent(null);
            localCurve.transform.position = new Vector3();*/
        }
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();

        if (target != null && Vector3.Distance(target.transform.position, transform.position) < 100 && Vector3.Dot((target.transform.position-transform.position).normalized, transform.forward)>0)
        {
            Vector3 targetPosition = target.transform.position + (target.Controller.travelingSpeed * target.Controller.Tangent) * (Mathf.Clamp(Vector3.Distance(weapon.transform.position, target.transform.position) / 100f, 0f, 1f));
            weapon.ShootPosition = targetPosition;
            weapon.Shoot = true;
            weapon.target = target.transform;
        }
        else
        {
            weapon.Shoot = false;
        }  

        //Movement
        if(localCurve != null)
        {
            distance += curveSpeed * Time.deltaTime;
            Vector3 tangent;
            Vector3 position = localCurve.CalcPositionAndTangentByDistance(distance, out tangent, true);
            Debug.Log(position);
            transform.localPosition = position;
            //transform.localRotation = Quaternion.LookRotation(tangent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SpaceShip ship = other.gameObject.GetComponent<SpaceShip>();
        if (ship != null)
        {
            target = ship;
        }
    }


}
