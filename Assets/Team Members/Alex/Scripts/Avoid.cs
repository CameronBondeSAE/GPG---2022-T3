using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace Alex
{
    public class Avoid : MonoBehaviour
    {
        public Rigidbody rb;

        public float turnSpeed;
        public float distance = 1f;

        // Start is called before the first frame update

        private void Awake()
        {
            Physics2D.queriesStartInColliders = false;
        }

        void FixedUpdate()
        {
            CheckForObject();
        }

        void CheckForObject()
        {
            RaycastHit hitInfo;


            if (Physics.Raycast(rb.transform.localPosition, transform.forward, out hitInfo, distance, 255))
            {
                Debug.DrawRay(rb.transform.localPosition, transform.forward * hitInfo.distance, Color.blue);

                if (hitInfo.distance <= 1f)
                {

                    //movement.speed = 10f;
                    rb.AddRelativeTorque(0, turnSpeed * 10, 0);
                    //transform.InverseTransformVector(hitInfo.transform.position);
                }

                if (hitInfo.distance <= 1.5f)
                {
                    //movement.speed = 10f;
                    rb.AddTorque(0, turnSpeed * 5, 0);
                    //transform.InverseTransformVector(hitInfo.transform.position);
                }

                else if (hitInfo.distance <= 2f)
                {
                    //movement.speed = 50f;
                    rb.AddTorque(0, turnSpeed, 0);
                    //transform.InverseTransformVector(hitInfo.transform.position);
                }
            }
        }
    }
}
