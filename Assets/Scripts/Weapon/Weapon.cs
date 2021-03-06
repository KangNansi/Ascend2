﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Rigidbody bullet;
    public GameObject burstParticle = null;
    public float frequency = 10;
    public float bulletSpeed = 1000;
    public Vector3 baseSpeed = new Vector3();
    public bool directionnal = false;
    public bool position = false;
    Vector3 shootDirection = new Vector3();
    Vector3 shootPosition = new Vector3();
    public Vector3 Direction
    {
        get
        {
            return shootDirection;
        }
        set
        {
            shootDirection = value;
        }
    }
    public Vector3 ShootPosition
    {
        set
        {
            shootPosition = value;
        }
    }

    bool shooting = false;
    float timer = 0.0f;
    public Transform target = null;

    public bool Shoot
    {
        get
        {
            return shooting;
        }
        set
        {
            shooting = value;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > 1/frequency && shooting)
        {
            if(burstParticle != null)
            {
                AutoDestroy.SpawnObject(burstParticle, transform.position, Quaternion.identity, 1f);
            }
            ShootBullet();
            timer = 0;
        }
	}

    void ShootBullet()
    {
        Rigidbody rigid = AutoDestroy.SpawnObject(bullet.gameObject, transform.position, Quaternion.identity, 5f).GetComponent<Rigidbody>();
        //Rigidbody rigid = (Rigidbody)Instantiate(bullet, transform.position, Quaternion.identity);
        //gameobject.transform.SetParent(transform);
        Vector3 direction = transform.forward;
        if(directionnal)
        {
            direction = shootDirection;
        }
        else if(position)
        {
            direction = (shootPosition - transform.position).normalized;
        }
        else if(target != null)
        {
            direction = (target.position - transform.position).normalized;
            Debug.DrawRay(transform.position, direction*1000, Color.blue, 0.2f);
        }
        rigid.velocity = direction * bulletSpeed + baseSpeed;
        //StartCoroutine(destroyObject(rigid.gameObject));
    }

    void ShootRay()
    {
        if (target == null)
            Debug.DrawRay(transform.position, transform.forward * 100, Color.red, (1 / frequency) / 2.0f);
        else
            Debug.DrawRay(transform.position, (target.position - transform.position).normalized * 1000, Color.red, (1 / frequency) / 2.0f);
    }

    IEnumerator destroyObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
