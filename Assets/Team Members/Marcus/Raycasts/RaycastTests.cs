using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class RaycastTests : MonoBehaviour
{
    public int bounces;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast for player direction
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        if()
        
        Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
    }

    public void BounceRaycast()
    {
        Vector3 reflect = Vector3.Reflect(radar01.direction, Hit01.normal);
        RaycastHit hitReflection;
        Physics.Raycast(Hit01.point, reflect, out hitReflection);
    }
}
