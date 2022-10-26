using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Radar : MonoBehaviour
{
    public int rays;
    private float raySpacing;

    private bool scanning;
    private float scanTimer;
    public float scanLength;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (scanning)
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer <= 0f)
            {
                RadialScan();
            }
            
            // Raycast for radar
            raySpacing = 360f / rays;
        
            for (int i = 0; i < rays; i++)
            {
                Vector3 facing = Quaternion.Euler(0, i * raySpacing, 0) * transform.forward;
                Debug.DrawRay(transform.position, facing * 10f, Color.green);
            }
        }
        
        // Raycast for player direction
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        
        Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
    }

    public void RadialScan()
    {
        scanning = !scanning;
        scanTimer = scanLength;
    } 
}
