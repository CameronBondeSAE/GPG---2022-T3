using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float radius;
    public Collider[] NearbyObjects;
    public GameObject TargetObject;
    Surround TargetScript;
    public int set;
    Transform targetSpot;
    public bool present;
    
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
            if (TargetObject.GetComponent<Surround>())
            {
                
                TargetScript = TargetObject.GetComponent<Surround>();
                set = TargetScript.SetNumber();
                targetSpot = TargetScript.Occupy(set);
                Debug.Log("Occupying");
                
                Vector3 direction = targetSpot.transform.position - transform.position;
                transform.Translate(direction * Time.deltaTime);
            }
        }
        else
        {
            present = false;
            foreach (var thing in NearbyObjects)
            {
                if (thing.GetComponent<Surround>())
                {
                    present = true;
                }
            }

            if (!present)
            {
                
                if (TargetScript != null) 
                {
                    TargetScript.Leave(set);
                    TargetScript = null; 
                    Debug.Log("Leaving");
                } 
                TargetObject = null;
                set = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
