using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanningRadar : MonoBehaviour
{
    public int rays;
    private float raySpacing;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        raySpacing = 20f / rays;
        
        for (int i = 0; i < rays; i++)
        {
            Vector3 scanDir = Quaternion.Euler(0, i * raySpacing, 0) * transform.forward;
            Debug.DrawRay(transform.position, scanDir * 10f, Color.green);
        }
    }
}
