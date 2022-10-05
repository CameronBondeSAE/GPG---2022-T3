using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class AlexAI : MonoBehaviour
{
    private SteeringBase align;
    private SteeringBase separation;
    private SteeringBase cohesion;
    Rigidbody rb;
    
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        align = GetComponent<Align>();
        separation = GetComponent<Separation>();
        cohesion = GetComponent<Cohesion>();
    }

    private void FixedUpdate()
    {
        GetComponent<SteeringManager>().CalculateMove(align.transform.position);
    }
}

