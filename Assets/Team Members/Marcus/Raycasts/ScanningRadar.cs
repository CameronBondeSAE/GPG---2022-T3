using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanningRadar : MonoBehaviour
{
    public int rays;
    private float raySpacing;

    private int cycles;
    private bool scanning;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        raySpacing = 20f / rays;

        if (cycles < 360 - rays && scanning)
        {
            for (int i = 0; i < rays; i ++)
            {
                Vector3 scanDir = Quaternion.Euler(0,  i * raySpacing + cycles, 0) * transform.forward;
                /*Debug.DrawRay(transform.position, scanDir * 10f, Color.green);*/
                Ray radar01 = new Ray(transform.position, scanDir);
                RaycastHit Hit01;
                Physics.Raycast(radar01, out Hit01);
                
                Vector3 reflect = Vector3.Reflect(radar01.direction, Hit01.normal);
                RaycastHit hitReflection;
                Physics.Raycast(Hit01.point, reflect, out hitReflection);

                Vector3 secondaryRef = Vector3.Reflect(reflect, hitReflection.normal);
                RaycastHit hitSecondary;
                Physics.Raycast(hitReflection.point, secondaryRef, out hitSecondary);
            }
            cycles++;
        }
        else if (cycles == 360 - rays && scanning)
        {
            StartScan();
        }

        // Raycast for player direction
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        
        Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
    }

    public void StartScan()
    {
        cycles = 0;
        scanning = !scanning;
    }
}
