using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTest : MonoBehaviour
{
    private Ray _ray;
    
    [SerializeField] float heightScale = 1.0f;

    // Distance covered per second along X axis of Perlin plane.
    [SerializeField]float xScale = 1.0f;

    [SerializeField]private float dist;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _ray = new Ray(transform.position, transform.forward*dist);
        
        Debug.DrawRay(transform.position, transform.forward*dist);

    }

    private void FixedUpdate()
    {
        float height = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);
        Vector3 pos = transform.position;
        pos.x = height;
        transform.position = pos;
    }
}
