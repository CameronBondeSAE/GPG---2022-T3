using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

public class Dive : MonoBehaviour
{
    private Rigidbody rb;

    private GameObject target;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Swoop>().target;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3)
        {
            rb.useGravity = true;
            rb.AddForce(transform.forward * 50);
        }
        else
        {
            transform.LookAt(target.transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}