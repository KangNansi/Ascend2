using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceEmmiter))]
[RequireComponent(typeof(ForceReceiver))]
public class Fire : MonoBehaviour {
    public Transform target;
    ForceEmmiter emmiter;
    ForceReceiver receiver;

	// Use this for initialization
	void Start () {
        emmiter = GetComponent<ForceEmmiter>();
        receiver = GetComponent<ForceReceiver>();
        receiver.burnable = false;
        emmiter.force.type = Force.Type.Fire;
	}
	
	// Update is called once per frame
	void Update () {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = target.position;
        if(emmiter.force.strength <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void setStrength(float s)
    {
        GetComponent<ForceEmmiter>().force.strength = s;
    }
}
