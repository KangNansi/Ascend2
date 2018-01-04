using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Link : MonoBehaviour {

    CharacterController controller = null;
    CharacterController Controller
    {
        get
        {
            if (!controller)
                controller = GetComponent<CharacterController>();
            return controller;
        }
    }

    //Links
    public CameraController cameraController;
    public Weapon weapon;

    //Parameters
    public float speed = 0.4f;
    public float jumpForce = 5f;
    public float jumpFading = 0.2f;

    //Variables
    Vector3 jumpVelocity = new Vector3();

	// Use this for initialization
	void Start () {
        cameraController.Follow(transform);
	}
	
	// Update is called once per frame
	void Update () {
		if(jumpVelocity.magnitude > 0f)
        {
            jumpVelocity *= (1 - (jumpFading * Time.deltaTime));
        }
	}

    public void Movement(float h, float v)
    {
        Controller.Move(cameraController.transform.rotation * new Vector3(h, 0, v) * speed + Physics.gravity + jumpVelocity);
    }

    public void Orientation(float h, float v)
    {
        cameraController.RotateAround(h, v);
    }

    public void Jump()
    {
        if(Controller.isGrounded)
        {
            jumpVelocity = jumpForce * Physics.gravity.magnitude * Vector3.up;
        }
    }

    public void Attack(bool value)
    {
        if(weapon)
        {
            weapon.Shoot = value;
        }
    }

}
