﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FootMan : MonoBehaviour {

    public Transform goal;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        
    }
	
	// Update is called once per frame
	void Update () {
        agent.SetDestination(goal.position);
    }
}
