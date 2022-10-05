using System;
using System.Collections;
using System.Collections.Generic;
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

            Vector3 averagePosition = Vector3.zero;

            // Average of all neighbours directions
            foreach (GameObject item in neighbours.neighbours)
            {
                averagePosition += item.transform.position;

            }

            averagePosition /= neighbours.neighbours.Count;

            Vector3 direction = (averagePosition - rb.transform.position).normalized;
            Debug.DrawLine(rb.transform.position, averagePosition, Color.green);
            return direction;
        }
    }
}