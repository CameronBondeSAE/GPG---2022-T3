using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Alex
{
	public class Align : SteeringBase 
	{
		Rigidbody rb;
		public Neighbours neighbours;
		public float force;

		private NativeArray<float3> _itemForwardVectors;
		private NativeArray<float3> _rotationForce;

		void Start()
		{
			rb = GetComponent<Rigidbody>();
			_itemForwardVectors = new NativeArray<float3>(100, Allocator.Persistent); //could use the total number of swarmers+aliens from game manager -1.
			_rotationForce = new NativeArray<float3>(1, Allocator.Persistent);
		}

		void FixedUpdate()
		{
			if (neighbours.neighbours.Count == 0) return;
			
			
			
			// Some are Torque, some are Force		
			CalculateMove();

			rb.AddTorque(_rotationForce[0] * force, ForceMode.Acceleration);
		}
		
		
		private void CalculateMove()
		{
			float3 forwardVector = transform.forward;

			neighbours.neighbours.RemoveAll(transformToTest => transformToTest == null);
			int numberOfNeighbours = neighbours.neighbours.Count;
			
			
			for (int i=0; i<numberOfNeighbours; i++)
			{
				_itemForwardVectors[i] = neighbours.neighbours[i].forward;
			}
			
			AlignJob job = new AlignJob
			{
				Forward = forwardVector,
				ItemPositions = _itemForwardVectors,
				Result = _rotationForce,
				NumberOfItems = numberOfNeighbours
			};
			
			JobHandle handle = job.Schedule();
			handle.Complete();
		}

		private void OnDestroy()
		{
			_itemForwardVectors.Dispose();
			_rotationForce.Dispose();
		}
	}
}
