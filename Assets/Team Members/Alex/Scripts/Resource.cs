using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alex
{

    public class Resource : MonoBehaviour
    {
        public bool isPickedUp;
        Rigidbody rb;
        public Inventory inventory;
        Vision vision;

        private void Awake()
        {
            
            rb = GetComponent<Rigidbody>();
            //isPickedUp = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 9)
            {
                inventory.resources += 1;
                //isPickedUp = true;
                Destroy(gameObject);
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            isPickedUp = false;
        }
    }
}
