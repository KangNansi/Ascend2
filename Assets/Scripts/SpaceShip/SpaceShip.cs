using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour {

    public float speed = 10;
    public Weapon weapon;
    public Weapon weaponB;

    public Rigidbody rigid;

    public bool Shooting
    {
        get
        {
            return weapon.Shoot;
        }
        set
        {
            weapon.Shoot = value;
            weaponB.Shoot = value;
        }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        weapon.directionnal = true;
        weaponB.directionnal = true;
    }

}
