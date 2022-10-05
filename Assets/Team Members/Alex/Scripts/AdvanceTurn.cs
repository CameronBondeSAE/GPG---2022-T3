using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdvanceTurn : MonoBehaviour
{
    public Rigidbody rb;
    public float numPoints;
    public float turnFraction;
    private void FixedUpdate()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1f);
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = 2 * Mathf.PI * turnFraction * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            Debug.Log(x);
            Debug.Log(y);
            Debug.Log(z);
            //return (x, y, z);
            //rb.AddRelativeTorque(0, point.x * turnSpeed, 0);
        }
    }

    
}
