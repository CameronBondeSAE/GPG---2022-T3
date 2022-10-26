using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class RaycastScanner : MonoBehaviour
{
    private int rays = 3;
    public float radarSpeed;
    private float timer;
    private Vector3 dir;
    
    RaycastHit hitInfo;

    void Update()
    {
        timer += Time.deltaTime * radarSpeed;
        if (timer >= 360f)
        {
            timer = 0f;
        }
        
        dir = Quaternion.Euler(0, timer, 0) * transform.forward;
        Physics.Raycast(transform.position, dir, out hitInfo);

        if (hitInfo.collider.GetComponent<PingObject>() != null)
        {
            StartCoroutine(hitInfo.collider.GetComponent<PingObject>().pinged());
            print("Ping");
        }
    }
    
    
}
