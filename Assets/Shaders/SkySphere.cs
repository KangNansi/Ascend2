using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SkySphere : MonoBehaviour {
    MeshRenderer renderer;
    MeshFilter filter;
    Material material;
    float timer = 0;
    public float dayDuration = 5;

    public Transform sunlight;

	// Use this for initialization
	void Start () {
        Generate();
        renderer = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        material = renderer.sharedMaterial;
    }

    public void Generate()
    {
        
    }

    // Update is called once per frame
    void Update () {
        transform.position = Camera.main.transform.position;
        
        timer += Time.deltaTime;
        if (timer > dayDuration)
            timer -= dayDuration;
        Vector3 sunPosition = Quaternion.AngleAxis(timer / dayDuration * 360, Vector3.right) * Vector3.up;
        sunlight.rotation = Quaternion.LookRotation(-sunPosition);
        material.SetVector("_Sun", sunPosition);
        material.SetFloat("_SkyState", Vector3.Dot(Vector3.up, sunPosition));
	}
}
