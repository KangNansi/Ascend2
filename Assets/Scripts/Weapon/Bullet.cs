using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject particle;
    public float damage = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageHandler = collision.gameObject.GetComponent<IDamageable>();
        if(damageHandler != null)
        {
            damageHandler.ReceiveDamage(damage);
        }
        Debug.Log("Destroyed");
        AutoDestroy.SpawnParticle(particle, transform.position, Quaternion.identity, 0.2f);
        Destroy(gameObject);
    }
}
