using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour {

    public Transform target = null;
    public float speed = 75f;
    public float damage = 100f;
    public GameObject explosion;
    Rigidbody rigid = null;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rigid.velocity = direction * speed;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageHandler = collision.gameObject.GetComponent<IDamageable>();
        if (damageHandler != null)
        {
            damageHandler.ReceiveDamage(damage);
        }
        Debug.Log("Destroyed");
        AutoDestroy.SpawnParticle(explosion, transform.position, Quaternion.identity, 0.2f).transform.SetParent(collision.transform);
        //PlanDestroy(0.2f);
        //if (rigid != null)
        //{
        //    rigid.isKinematic = true;
        //}
        Destroy(gameObject);
    }

    //IEnumerator PlanDestroy(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    Destroy(gameObject);
    //}
}
