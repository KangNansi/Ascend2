using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    public float duration = 1f;
    float time = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time>duration)
        {
            Destroy(gameObject);
        }
	}

    public static GameObject SpawnParticle(GameObject particle, Vector3 position, Quaternion rotation, float duration = 0.0f)
    {
        GameObject p = Instantiate<GameObject>(particle, position, rotation);
        ParticleSystem partS = p.GetComponent<ParticleSystem>();
        if(partS != null && duration == 0.0f)
        {
            duration = partS.main.duration;
        }
        AutoDestroy autod = p.AddComponent<AutoDestroy>();
        autod.duration = duration;
        return p;
    }

    public static GameObject SpawnObject(GameObject obj, Vector3 position, Quaternion rotation, float duration)
    {
        GameObject p = Instantiate<GameObject>(obj, position, rotation);
        AutoDestroy autod = p.AddComponent<AutoDestroy>();
        autod.duration = duration;
        return p;
    }
}
