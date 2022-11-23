using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Jobs;

namespace Alex
{
public class Separation : SteeringBase
{
    Rigidbody rb;
    public Neighbours neighbours;
    public float force;

    public NativeArray<float3> neighbourPositions;

    public bool steveJobs;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        neighbourPositions = new NativeArray<float3>(50, Allocator.Persistent);
    }

    void FixedUpdate()
    {
        // Some are Torque, some are Force
        rb.AddForce(CalculateMove() * force);
    }


    public Vector3 CalculateMove()
    {
        if (neighbours.neighbours.Count == 0)
            return Vector3.zero;

        Vector3 separationMove = Vector3.zero;

        #region Kevin's Job Work

        if (steveJobs)
        {
            float3 position = rb.transform.position;
        
            for (int i = 0; i < neighbours.neighbours.Count; i++)
            {
                neighbourPositions[i] = neighbours.neighbours[i].position;
            }
        
            SeparationJob separationJobs = new SeparationJob()
            {
                NumberOfNeighbours = neighbours.neighbours.Count,
                NeighbourPosition = neighbourPositions,
                MyPosition = position,
            };
            JobHandle handle = separationJobs.Schedule();
            handle.Complete();
            return separationJobs.NormalizedDirection;
        }
        #endregion
        else
        {
            foreach (Transform item in neighbours.neighbours)
            {
                separationMove += item.position;
            }

            separationMove /= neighbours.neighbours.Count;

            Vector3 direction = (rb.transform.position - separationMove).normalized;

            //Debug.DrawLine(rb.transform.localPosition, separationMove, Color.red);

            return direction;
        }
        // Average of all neighbours directions
    }

    public void OnDestroy()
    {
        neighbourPositions.Dispose();
    }
}
}
