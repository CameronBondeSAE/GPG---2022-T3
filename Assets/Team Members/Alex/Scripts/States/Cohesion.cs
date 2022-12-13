using System;
using System.Collections;
using System.Collections.Generic;
using Ollie;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Alex
{
    public class Cohesion : SteeringBase
    {
        Rigidbody rb;
        public Neighbours neighbours;
        public float force;


        #region Ollie's Job Stuff

        [Header ("Ollie's Job Settings")]
        public bool runningAsJob;
        public NativeArray<float3> averagePosition;
        public NativeArray<float3> neighbourPositions;
        public NativeArray<float3> myPos;
        public NativeArray<float3> direction;
        public NativeArray<int> neighboursCount;

        void Awake()
        {
            //toggle this true for jobs, false for Alex's base code
            runningAsJob = false;
            
            //arbitrary length because actual length might change
            neighbourPositions = new NativeArray<float3>(100, Allocator.Persistent);
            
            //only a single variable so locking length to 1
            averagePosition = new NativeArray<float3>(1, Allocator.Persistent);
            myPos = new NativeArray<float3>(1, Allocator.Persistent);
            direction = new NativeArray<float3>(1, Allocator.Persistent);
            neighboursCount = new NativeArray<int>(1, Allocator.Persistent);
        }

        public void OnDestroy()
        {
            averagePosition.Dispose();
            neighbourPositions.Dispose();
            myPos.Dispose();
            direction.Dispose();
            neighboursCount.Dispose();
        }

        #endregion

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            // Some are Torque, some are Force
            //rb.AddTorque(CalculateMove()*force);
            rb.AddForce(CalculateMove() * force);
        }


        public Vector3 CalculateMove()
        {
            if (neighbours.neighbours.Count == 0)
                return Vector3.zero;

            neighbours.neighbours.RemoveAll(transformToTest => transformToTest == null);
            #region Ollie's Jobify Attempt
            if (runningAsJob)
            {
                //Assign your own current variables
                this.averagePosition[0] = Vector3.zero;
                for (int i = 0; i < neighbours.neighbours.Count; i++)
                {
                    neighbourPositions[i] = neighbours.neighbours[i].position;
                }
                myPos[0] = transform.position;
                neighboursCount[0] = neighbours.neighbours.Count;
            
                //Create job framework, setting it's parameters as your own current variables from above
                CohesionJob cohesionJob = new CohesionJob
                {
                    averagePosition = this.averagePosition,
                    neighbourPositions = this.neighbourPositions,
                    myPos = this.myPos,
                    direction = this.direction,
                    neighboursCount = this.neighboursCount,
                };

                //Start jobs according to number of neighboursCount
                //JobHandle cohesionHandle = cohesionJob.Schedule(neighboursCount[0], 1);
                JobHandle cohesionHandle = cohesionJob.Schedule();
                cohesionHandle.Complete();

                
                Vector3 jobDirection = this.direction[0];
                jobDirection = jobDirection.normalized;
                return jobDirection;
                
            }
            #endregion
            
            else
            {
                Vector3 averagePosition = Vector3.zero;

                // Average of all neighbours directions
                foreach (Transform item in neighbours.neighbours)
                {
                    averagePosition += item.position;

                }

                averagePosition /= neighbours.neighbours.Count;

                Vector3 direction = (averagePosition - rb.transform.position).normalized;
                //Debug.DrawLine(rb.transform.position, averagePosition, Color.green);
                return direction;
            }
        }
    }
}