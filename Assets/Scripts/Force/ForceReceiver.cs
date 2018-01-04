using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour {

    public Fire firePrefab;

    public delegate void ReceiveForce(Force force);
    public event ReceiveForce receiveForce;

    float temperature = 0f;
    public bool burnable = false;
    public float burnTemperature = 50f;
    Fire fire;
    bool burning = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!burning && burnable && temperature >= burnTemperature)
        {
            fire = Instantiate<Fire>(firePrefab);
            fire.target = transform;
            fire.transform.localScale = transform.lossyScale;
            fire.setStrength(temperature*2);
            burning = true;
        }
        else if(fire && temperature < burnTemperature/2f)
        {
            Destroy(fire.gameObject);
            burning = false;
        }
        if(temperature > 20)
        {
            temperature -= 5 * Time.deltaTime;
        }
	}

    public void Receive(Force force)
    {
        if(receiveForce != null)
        {
            receiveForce(force);
        }
        //GetComponent<Rigidbody>().AddForce(force.rigidBody.velocity.normalized*force.strength);
    }

    public void TargetTemperature(float temp)
    {
        temperature += (temp - temperature) / 2f;
    }

    public void AddTemperature(float temp)
    {
        temperature += temp;
    }
}
