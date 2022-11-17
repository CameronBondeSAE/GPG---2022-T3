using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
public class Separation : SteeringBase
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
        rb.AddForce(CalculateMove() * force);
    }


    public Vector3 CalculateMove()
    {
        if (neighbours.neighbours.Count == 0)
            return Vector3.zero;

        Vector3 separationMove = Vector3.zero;

        // Average of all neighbours directions
        foreach (Transform item in neighbours.neighbours)
        {
            separationMove += item.position;
        }

        separationMove /= neighbours.neighbours.Count;

        Vector3 direction = (rb.transform.position - separationMove).normalized;

        //Debug.DrawLine(rb.transform.localPosition, separationMove, Color.red);

        return direction;
    }

}
}
