using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int resources;
    public bool capacityReached;
    Sensor sensor;

    private void Start()
    {
        sensor = GetComponent<Sensor>();
        resources = 0;
    }

    public void Update()
    {
        if (resources >= 5)
        {
            resources = 5;
            capacityReached = true;
            //Debug.Log("Heading to drop off point");
        }
    } 
}
