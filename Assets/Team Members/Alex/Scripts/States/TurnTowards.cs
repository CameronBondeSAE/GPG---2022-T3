using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
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
        public Wander wander;
        public AStar astar;
        public FollowPath followPath;
        public float slowingForce = 50f;
        public float slowDownAngleThreshhold = 25f;

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
            /*
            Vector3 point;
            //targetTransform = vision.resourcesInSight[0];
                
                point = transform.InverseTransformPoint(targetPosition);
                rb.AddRelativeTorque(0, point.x * turnSpeed, 0);
                
                Vector3 targetDir = targetPosition - transform.position;
                float angle = Vector3.Angle(transform.forward,targetDir);
                rb.AddRelativeForce(0, 0, -angle * slowingForce);
                rb.AddRelativeTorque(0, point.x * turnSpeed, 0);
                */
                
                if (targetTransform)
                {
                    targetPosition = targetTransform.position;
                    
                }
                
                Vector3 targetDir = targetPosition - transform.position;
                float angle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);
                
                
                
                // Slow down if facing away from target
                if(Mathf.Abs(angle) > slowDownAngleThreshhold)
                    rb.AddRelativeForce(0, 0,  -Mathf.Abs(angle * slowingForce));
                rb.AddRelativeTorque(0, angle * turnSpeed, 0);
                
        }
    }
}