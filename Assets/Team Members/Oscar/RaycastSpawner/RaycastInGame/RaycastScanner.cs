using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScanner : MonoBehaviour
{
    public int rayAmount = 360;
    public int spacingScale;
    // Update is called once per frame

    void Update()
    {
        Ray ray = new Ray(Quaternion.Euler(0,360*spacingScale,0) * transform.position, transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);

        Vector3 dir = Quaternion.Euler(0, 1 + spacingScale, 0) * transform.forward;

        for (int i = 0; i < rayAmount; i++)
        {
            Debug.DrawLine(ray.origin,dir + hitInfo.point,Color.green);
        }
        
        
    }
}
