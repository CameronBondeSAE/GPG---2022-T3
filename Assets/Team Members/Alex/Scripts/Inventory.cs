using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int resources;
    public bool capacityReached;
    private Sensor sensor;

    private void Start()
    {
        resources = 0;
    }

    public void Update()
    {
        if (resources >= 10)
        {
            capacityReached = true;
            resources = 10;
        }
    } 
}
