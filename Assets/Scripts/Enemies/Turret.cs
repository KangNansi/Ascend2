using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy {
    public Weapon weapon;
    public Transform cannonPivot;
    SpaceShip target = null;

	// Use this for initialization
	new void Start () {
        base.Start();

	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();

        if(target != null)
        {
            weapon.Shoot = true;
            weapon.target = target.transform;
            cannonPivot.LookAt(target.transform);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        SpaceShip ship = other.gameObject.GetComponent<SpaceShip>();
        if(ship != null)
        {
            target = ship;
        }
    }
}
