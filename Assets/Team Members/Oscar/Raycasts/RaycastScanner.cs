using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaycastScanner : ImmediateModeShapeDrawer
{
    public float radarSpeed = 100f;
    private float timer;
    private Vector3 dir;
    
    RaycastHit hitInfo;
    
    private float rays = 1;

    private void Update()
    {
        timer += Time.deltaTime * radarSpeed;
        if (timer >= 360f)
        {
            timer = 0f;
        }
        
        dir = Quaternion.Euler(0, timer, 0) * transform.forward;
    
        Physics.Raycast(transform.position, dir, out hitInfo,10f);
        
        if (hitInfo.collider.GetComponent<PingObject>() != null)
        {
            //StartCoroutine(hitInfo.collider.GetComponent<PingObject>().pinged());
            print("Ping");
        }
    }
}
