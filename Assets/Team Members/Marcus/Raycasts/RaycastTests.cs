using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class RaycastTests : MonoBehaviour
{
    public float rayDistance;
    public int bounces;

    private Vector3 newOrigin;
    private Vector3 newDirection;
    private Vector3 reflection;
    private RaycastHit newHit;
    
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

        if (Physics.Raycast(ray, out hitInfo, rayDistance))
        {
            reflection = Vector3.Reflect(ray.direction, hitInfo.normal);
            newOrigin = hitInfo.point; newDirection = reflection;
            
            BounceRaycast();
        }
        
        Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
    }

    public void BounceRaycast()
    {
        for (int x = 0; x < bounces; x++)
        {
            Ray newRay = new Ray(newOrigin, newDirection);
            if (Physics.Raycast(newRay, out newHit, rayDistance))
            {
                reflection = Vector3.Reflect(newRay.direction, newHit.normal);
                newOrigin = newHit.point; newDirection = reflection;
            }
        }
    }
}
