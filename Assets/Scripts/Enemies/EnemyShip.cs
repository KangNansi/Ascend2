using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShip : Enemy {

    public Weapon weapon;


    SpaceShip target = null;



	// Use this for initialization
	new void Start () {
        base.Start();

	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
        if (target != null && Vector3.Distance(target.transform.position, transform.position) < 100)// && Vector3.Dot(target.transform.position-transform.position, transform.forward)>0)
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


    }


}
