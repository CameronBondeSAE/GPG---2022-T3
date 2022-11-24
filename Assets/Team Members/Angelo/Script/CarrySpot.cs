using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrySpot : MonoBehaviour
{
    public int id;
    public float radius = 0.25f;
    public Collider[] NearbyObjects;
    private Surround parentScript;
    // Start is called before the first frame update
    void Start()
    {
        parentScript = GetComponentInParent<Surround>();
    }

    // Update is called once per frame
    void Update()
    {
        NearbyObjects = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider thing in NearbyObjects)
        {
            if (thing.GetComponent<Pickup>())
            {
                float dist = Vector3.Distance(thing.transform.position, transform.position);
                if (radius >= dist && id == thing.GetComponent<Pickup>().set)
                {
                    parentScript.counter += 1;
                }
            }
        }
    }
}
