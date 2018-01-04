using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceEmmiter))]
[RequireComponent(typeof(ForceReceiver))]
public class Wind : MonoBehaviour {

    ForceReceiver receiver;
    ForceEmmiter emmiter;

	// Use this for initialization
	void Start () {
        receiver = GetComponent<ForceReceiver>();
        emmiter = GetComponent<ForceEmmiter>();
        receiver.receiveForce += ReceiveForce;
	}
	
	// Update is called once per frame
	void Update () {
        emmiter.force.strength -= 5f * Time.deltaTime;
		if(emmiter.force.strength <= 0)
        {
            Destroy(gameObject);
        }
	}

    void ReceiveForce(Force f)
    {
        if(f.type == Force.Type.Fire)
        {
            receiver.AddTemperature(100);
        }
    }
}
