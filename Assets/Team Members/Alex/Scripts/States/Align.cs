using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
	public class Align : SteeringBase 
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
			Vector3 targetDirection = CalculateMove();

			// Cross will take YOUR direction and the TARGET direction and turn it into a rotation force vector
			Vector3 cross = Vector3.Cross(transform.forward, targetDirection);

			rb.AddTorque(cross * force);
		}
		
		
		public Vector3 CalculateMove()
		{
			if (neighbours.neighbours.Count == 0)
				return Vector3.zero;

			Vector3 alignmentMove = Vector3.zero;

			// Average of all neighbours directions
			foreach (Transform item in neighbours.neighbours)
			{
				alignmentMove += item.forward;
			}

			alignmentMove /= neighbours.neighbours.Count;

			return alignmentMove;
		}
	}
}
