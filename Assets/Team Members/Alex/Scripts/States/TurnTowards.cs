using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class TurnTowards : MonoBehaviour
    {
        public Transform targetTransform;
        public Vector3 targetPosition;
        public int turnSpeed;

        Rigidbody rb;
        // Start is called before the first frame update

        void Start()
        {
            rb = GetComponent<Rigidbody>();

            //if (target != null) 
            //hacked up for now needs to be written better since this script will be used for multiple different targets
            //target = GameObject.Find("Target").transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 point;
            if (targetTransform != null)
            {
                point = transform.InverseTransformPoint(targetTransform.transform.position);
            }
            else
            {
                point = transform.InverseTransformPoint(targetPosition);
            }
            rb.AddRelativeTorque(0, point.x * turnSpeed, 0);
        }
    }
}