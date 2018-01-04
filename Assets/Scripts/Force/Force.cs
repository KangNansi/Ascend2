using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Force {

    public enum Type { Wind, Fire, Magnetic };

    public Transform transform;
    public Type type;
    public Vector3 direction = Vector3.forward;
    public float strength;
    public Rigidbody rigidBody;

    public void Apply(ForceReceiver receiver)
    {
        Rigidbody rcvrBody = receiver.GetComponent<Rigidbody>();
        switch (type)
        {
            case Type.Wind:
                if(rcvrBody != null)
                {
                    rcvrBody.AddForce(rigidBody.velocity.normalized * strength);
                }
                break;
            case Type.Fire:
                float transmission = strength * Time.deltaTime;
                receiver.AddTemperature(strength);
                //strength -= transmission;
                break;
            case Type.Magnetic:
                if (rcvrBody != null)
                {
                    Vector3 distance = (rcvrBody.transform.position - transform.position);
                    float magnitude = distance.magnitude;
                    Vector3 direction = distance.normalized;
                    rcvrBody.AddForce(-direction * (strength/magnitude));
                }
                break;
        }
    }
}
