using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Leader;
    public float SpaceBetween = 2.0f;
    public float SpeedCap = 10;
    void Start()
    {
        
    }

    
    void Update()
    {
        float dist = Vector3.Distance(Leader.position, transform.position);
        if (dist > SpaceBetween)
        {
            Vector3 direction = Leader.position - transform.position;
            direction = direction * Time.deltaTime;
            transform.Translate(direction);   
        }
    }
}
