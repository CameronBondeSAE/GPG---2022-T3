using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScanner : MonoBehaviour
{
    public int rayAmount = 360;
    int spacingScale;
    // Update is called once per frame

    void Update()
    {
        for (int i = 0; i < rayAmount; i++)
        {
            Ray ray = new Ray(Quaternion.Euler(0,360*spacingScale,0) * transform.position, transform.forward);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            Debug.DrawLine(ray.origin,hitInfo.point,Color.green);
        }
        
    }
}
