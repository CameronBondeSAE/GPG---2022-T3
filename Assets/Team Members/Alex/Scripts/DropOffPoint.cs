using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    Sensor sensor;
    Inventory inventory;
    Controller controller;
    Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sensor = GetComponent<Sensor>();
        controller = GetComponent<Controller>();
        inventory = GetComponent<Inventory>();
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 9)
            {
                Inventory inventory = collision.gameObject.GetComponent<Inventory>();
                inventory.resources = 0;
                inventory.capacityReached = false;
            }
        }
}
