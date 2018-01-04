using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForceEmmiter : MonoBehaviour {
    public Force force;

	// Use this for initialization
	void Start () {
        force.transform = transform;
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnCollisionStay(Collision collision)
    {
        ForceReceiver receiver = collision.collider.GetComponent<ForceReceiver>();
        if(receiver)
        {
            receiver.Receive(force);
            ApplyOn(receiver);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        ForceReceiver receiver = other.GetComponent<ForceReceiver>();
        if (receiver)
        {
            receiver.Receive(force);
            ApplyOn(receiver);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ForceReceiver receiver = other.GetComponent<ForceReceiver>();
        if (receiver)
        {
            receiver.Receive(force);
            ApplyOn(receiver);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ForceReceiver receiver = collision.collider.GetComponent<ForceReceiver>();
        if (receiver)
        {
            receiver.Receive(force);
            ApplyOn(receiver);
        }
    }

    void ApplyOn(ForceReceiver f)
    {
        force.Apply(f);
    }
}
