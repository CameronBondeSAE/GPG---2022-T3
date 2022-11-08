using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alex
{

    public class Resource : MonoBehaviour
    {
        
        Rigidbody rb;
        Vision vision;
        Inventory inventory;
        public LayerMask layerMask;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            rb = GetComponent<Rigidbody>();
            //isPickedUp = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AlienAI"))
            {
                Inventory inventory = collision.gameObject.GetComponent<Inventory>();
                inventory.resources += 1;
                Vision vision = collision.gameObject.GetComponent<Vision>();
                vision.resourcesInSight.Remove(transform);
                Destroy(gameObject);
            }
        }
    }
}
