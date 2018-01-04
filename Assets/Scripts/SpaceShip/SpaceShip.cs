using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour, IDamageable {

    public delegate void ShipEvent();
    public event ShipEvent OnDamage;

    public GameObject explosion;
    public float speed = 10;
    public float maxLife = 100;
    public bool receiveDamage = true;
    ShipController controller;
    public ShipController Controller
    {
        get
        {
            return controller;
        }
        set
        {
            controller = value;
        }
    }
    float life;
    public float Life
    {
        get
        {
            return life;
        }
    }
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
        life = maxLife;
    }

    public bool ReceiveDamage(float value)
    {
        if(!receiveDamage)
        {
            return true;
        }
        if(OnDamage != null)
        {
            OnDamage();
        }
        life -= value;
        if(life <= 0)
        {
            AutoDestroy.SpawnParticle(explosion, transform.position, Quaternion.identity, 2f);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
        return false;
    }
}
