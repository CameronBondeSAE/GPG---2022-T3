using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    Sensor sensor;
    Inventory inventory;
    Controller controller;

    public void Awake()
    {
        sensor = GetComponent<Sensor>();
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 9)
            {
                controller.hasResource = false;
                inventory.resources = 0;
            }
        }
}
