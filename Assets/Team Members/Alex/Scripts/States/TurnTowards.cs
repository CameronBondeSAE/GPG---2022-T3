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
        public Vision vision;
        public Controller controller;
        public Wonder wonder;
        public AStar astar;
        public FollowPath followPath;
        public float slowingForce = 50f;

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
            //targetTransform = vision.resourcesInSight[0];
                
                point = transform.InverseTransformPoint(targetPosition);
                rb.AddRelativeTorque(0, point.x * turnSpeed, 0);
                // Slow down if facing away from target
                // Get angle....
                
                
                    Vector3 targetDir = targetPosition - transform.position;
                    float angle = Vector3.Angle(targetDir, transform.forward);
                    rb.AddRelativeForce(0, 0, -angle * slowingForce);
            
        }
    }
}