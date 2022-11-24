using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float radius;
    public float speed;
    public Collider[] NearbyObjects;
    public GameObject TargetObject;
    Surround TargetScript;
    public int set;
    Transform targetSpot;
    public bool present;
    private bool tick;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float MinDistance = 99999999;
        NearbyObjects = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider thing in NearbyObjects)
        {
            if (thing.GetComponent<Surround>())
            {
                float dist = Vector3.Distance(thing.transform.position, transform.position);
                if (MinDistance > dist)
                {
                    MinDistance = dist;
                    TargetObject = thing.gameObject;
                }
            }
        }

        if (TargetObject != null && NearbyObjects != null && present)
        {
            if (TargetObject.GetComponent<Surround>() && !tick)
            {
                tick = true;
                TargetScript = TargetObject.GetComponent<Surround>();
                set = TargetScript.SetNumber();
                targetSpot = TargetScript.Occupy(set);
            }

            if (Vector3.Distance(targetSpot.transform.position, transform.position) > 0.2)
            {
                Vector3 direction = targetSpot.transform.position - transform.position;
                direction.Normalize();
                transform.Translate(direction * Time.deltaTime * speed);
                transform.LookAt(TargetObject.transform.position);
            }
        }
    


        present = false;
        foreach (var thing in NearbyObjects)
        {
            if (thing.GetComponent<Surround>()) 
            { 
                present = true; 
                break;
            }
        }

        if (!present)
        {
            if (TargetScript != null) 
            { 
                TargetScript.Leave(set); 
                TargetScript = null;
                tick = false;
            } 
            TargetObject = null;
            set = 0;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
