using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PerlinTest : MonoBehaviour
{
    private Ray _ray;

    private Rigidbody _rb;
    
    [SerializeField] float heightScale = 1.0f;

    // Distance covered per second along X axis of Perlin plane.
    [SerializeField]float xScale = 1.0f;

    [SerializeField]private float dist;
    
    Quaternion _currentRotation;

    // Update is called once per frame
    void Update()
    {
        _rb = GetComponent<Rigidbody>();
        
        _ray = new Ray(transform.position, transform.forward*dist);
        
        Debug.DrawRay(transform.position, transform.forward*dist);

    }

    private void FixedUpdate()
    {
        /*float height = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);
        Vector3 pos = transform.position;
        pos.x = height;
        transform.position = pos;*/

        Wobble();
    }
    
    private void Wobble()
    {
        float height = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);

        Vector3 _angleVector = new Vector3(0, height, 0);

        _currentRotation.eulerAngles = _angleVector;

       _rb.MoveRotation(_currentRotation);
    }
}
