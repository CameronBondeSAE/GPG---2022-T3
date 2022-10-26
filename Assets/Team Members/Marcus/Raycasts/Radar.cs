using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        
        Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
    }
}
