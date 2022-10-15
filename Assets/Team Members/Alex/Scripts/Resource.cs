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
        [SerializeField]
        Vision vision;

        private Sensor sensor;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            isPickedUp = false;
        }
        
        /*
        private void OnTriggerEnter(Collider other)
        {
            
            if(other.gameObject.CompareTag("Player"));
            {
                isPickedUp = true;
                Destroy(gameObject);
            }
        }
*/
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 9)
            {
                isPickedUp = true;
                Destroy(gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            isPickedUp = false;
        }
    }
}
