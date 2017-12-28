using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject particle;
    public float damage = 10;
    Rigidbody rigid = null;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
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
        PlanDestroy(0.2f);
        if(rigid != null)
        {
            rigid.isKinematic = true;
        }
    }

    IEnumerator PlanDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
