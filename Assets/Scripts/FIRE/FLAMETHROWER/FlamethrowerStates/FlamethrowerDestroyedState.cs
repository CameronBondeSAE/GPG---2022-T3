using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Unity.VisualScripting;
using UnityEngine;

public class FlamethrowerDestroyedState : MonoBehaviour
{
    private FlamethrowerModel model;

    private Rigidbody rb;

    private void OnEnable()
    {
        model = GetComponent<FlamethrowerModel>();
        model.enabled = false;
        
        
        this.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.mass = 20;
        rb.AddForce(new Vector3(0, 10f, 0));
    }
}
