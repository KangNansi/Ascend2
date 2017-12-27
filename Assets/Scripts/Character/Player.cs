using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public delegate bool OnUpdate(float deltaTime);
    public class State
    {
        public float time = 0.0f;

        public OnUpdate update = null;
    }

    CharacterController controller;

    new public Camera camera;
    public Weapon weapon;
    public float Speed = 0.5f;
    public float RotateSpeed = 5.0f;

    bool canMove = true;

    State currentState = new State();

    Vector3 dashDir = new Vector3();
    public float dashSpeed = 10;
    public float dashDistance = 5;
    float dashBuffer = 0;

    Transform target = null;
    public float targetRadius = 100;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float hr = Input.GetAxis("HorizontalR");
        float vr = Input.GetAxis("VerticalR");

        Quaternion torque = Quaternion.AngleAxis(hr * RotateSpeed, transform.up);
        lookUp(-vr);

        weapon.Shoot = Input.GetAxis("Fire1")>0.1f;
        if(Input.GetButton("Dash"))
        {
            Dash(h * transform.right + v * transform.forward);
        }
        if(Input.GetButtonDown("Target"))
        {
            if(target == null)
            {
                target = FindTarget();
                weapon.target = target;
            }
            else
            {
                target = null;
                weapon.target = null;
            }
        }


        if(target == null)
        {
            transform.rotation *= torque;
        }
        else
        {
            transform.LookAt(target);
        }


        if(canMove)
        {
            Vector3 motion = h * transform.right + v * transform.forward;
            controller.Move(motion * Speed);
        }
        if(currentState.update != null)
        {
            if(currentState.update(Time.deltaTime))
            {
                currentState = new State();
            }
        }
        controller.Move(Vector3.down);
	}

    void lookUp(float deltaMove)
    {
        Vector3 arm = camera.transform.position - transform.position;
        /*float distance = arm.magnitude;
        arm.Normalize();*/
        arm = Quaternion.AngleAxis(deltaMove, transform.right) * arm;
        camera.transform.position = transform.position + arm;// * distance;
        camera.transform.LookAt(transform);
    }

    bool OnDashUpdate(float deltaTime)
    {
        currentState.time += deltaTime;
        controller.Move(dashDir * dashSpeed);
        dashBuffer += dashSpeed;
        if(currentState.time > 1.0f || dashBuffer > dashDistance)
        {
            canMove = true;
            return true;
        }
        return false;
    }

    void Dash(Vector3 direction)
    {
        State dash = new State();
        dash.update = OnDashUpdate;
        currentState = dash;
        dashDir = direction;
        canMove = false;
        dashBuffer = 0;
    }

    Transform FindTarget()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, targetRadius);
        Transform best = null;
        float bestDotValue = -1;
        foreach(Collider c in results)
        {
            if(c.GetComponent<Target>() == null)
            {
                continue;
            }
            Vector3 dirToTarget = c.transform.position - transform.position;
            dirToTarget.Normalize();
            float dotValue = Vector3.Dot(transform.forward, dirToTarget);
            if(best == null || dotValue > bestDotValue)
            {
                best = c.transform;
                bestDotValue = dotValue;
            }
        }
        return best;
    }

}
