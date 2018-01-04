using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public enum CameraMode { None, Follow, LookAt};

    public Transform target = null;

    public CameraMode mode = CameraMode.None;

    Quaternion baseRotation;

    //Paramètres
    //Follow
    public float minDistance = 3f;
    public float maxDistance = 3f;
    public float followPercent = 0.5f;
    public float lookAtPercent = 0.8f;
    public float rotateSpeed = 10f;

    //Shake
    public float shakeSpeed = 5f;
    public float shakeFade = 0.8f;
    float trauma = 0;
    float shakeTime = 0;
    Quaternion shaker;

	// Use this for initialization
	void Start () {
        baseRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        float shake1 = Mathf.PerlinNoise(shakeTime, shakeTime)-0.5f;
        float shake2 = Mathf.PerlinNoise(7777 + shakeTime, 7777 + shakeTime)-0.5f;
        shakeTime += Time.deltaTime * shakeSpeed;
        shaker = (Quaternion.AngleAxis(shake1 * trauma, Vector3.up)
            *Quaternion.AngleAxis(shake2 * trauma, Vector3.right));
        trauma -= Time.deltaTime * shakeFade;
        if (trauma < 0) trauma = 0;

        switch (mode)
        {
            case CameraMode.None:
                transform.rotation = Quaternion.Slerp(transform.rotation, baseRotation, lookAtPercent * Time.deltaTime);
                break;

            case CameraMode.Follow:
                FollowUpdate();
                break;

            case CameraMode.LookAt:
                transform.LookAt(target);
                break;
        }

        transform.rotation *= shaker;
	}

    void FollowUpdate()
    {
        if(target != null)
        {
            Vector3 distance = ((target.position+Vector3.up*3) - transform.position);
            Vector3 direction = distance.normalized;
            float magnitude = distance.magnitude;
            Vector3 motion = new Vector3();
            Vector3 targetPosition = distance;
            if(magnitude>maxDistance)
            {
                targetPosition = direction * (magnitude - maxDistance);
                motion = targetPosition * followPercent * Time.deltaTime;
            }
            else if(magnitude<minDistance)
            {
                targetPosition = direction * (magnitude - minDistance);
                motion = (targetPosition * followPercent * Time.deltaTime);
            }
            transform.position += motion;
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), lookAtPercent*Time.deltaTime);
            transform.rotation = rotation;
        }
    }

    public void Follow(Transform t)
    {
        target = t;
        mode = CameraMode.Follow;
    }

    public void RotateAround(float h, float v)
    {
        transform.RotateAround(target.position, Vector3.up, h*Time.deltaTime*rotateSpeed);
        transform.RotateAround(target.position, transform.right, v * Time.deltaTime * rotateSpeed);
    }

    public void Shake(float traumaValue)
    {
        trauma += traumaValue;
    }
}
