using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Rigidbody bullet;
    public float frequency = 10;
    public float bulletSpeed = 1000;

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
            ShootBullet();
            timer = 0;
        }
	}

    void ShootBullet()
    {
        Rigidbody gameobject = (Rigidbody)Instantiate(bullet, transform.position, transform.rotation);
        Vector3 direction = transform.forward;
        if(target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
        gameobject.velocity = direction * bulletSpeed;
        StartCoroutine(destroyObject(gameobject.gameObject));
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
