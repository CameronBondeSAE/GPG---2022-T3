using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Enemy : MonoBehaviour
    {


        Rigidbody rb;
        Vision vision;
        Inventory inventory;

        private void Awake()
        {
            //vision = GetComponent<Vision>();
            //inventory = GetComponent<Inventory>();
            //rb = GetComponent<Rigidbody>();
            GetComponent<Health>().YouDied += killYourself;
            //isPickedUp = false;
        }

        private void killYourself()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6)
            {
                Destroy(gameObject);
            }
        }
    }
}