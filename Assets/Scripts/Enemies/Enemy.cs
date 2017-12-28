using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {

    public GameObject Explosion;

    public float maxLife = 100;
    float life = 100;

    public void ReceiveDamage(float value)
    {
        life -= value;
        Debug.Log("Damaged");
    }

    // Use this for initialization
    protected void Start () {
        life = maxLife;
	}
	
	// Update is called once per frame
	protected void Update () {
		if(life < 0)
        {
            OnDie();
        }
	}

    protected virtual void OnDie()
    {
        AutoDestroy.SpawnParticle(Explosion, transform.position, Quaternion.identity, 1f);
        Destroy(gameObject);
    }

    static public IEnumerator SpawnParticle(GameObject particle, Vector3 position, float duration)
    {
        GameObject p = Instantiate<GameObject>(particle, position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(p);
    }
}
