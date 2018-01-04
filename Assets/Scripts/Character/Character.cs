using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public Link link;

    float h, v;
    float hr, vr;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        hr = Input.GetAxis("HorizontalR");
        vr = Input.GetAxis("VerticalR");

        link.Movement(h, v);
        link.Orientation(hr, vr);

        if(Input.GetButtonDown("Jump"))
        {
            link.Jump();
        }

        link.Attack(Input.GetAxis("Fire1") > 0.7);
	}
}
