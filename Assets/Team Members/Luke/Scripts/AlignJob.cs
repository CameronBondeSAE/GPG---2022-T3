using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Services.Analytics;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public struct AlignJob : IJob
{
	public NativeArray<float3> Result;

	[ReadOnly]
	public NativeArray<float3> ItemPositions;

	public float3 Forward;
	public int NumberOfItems;

	public void Execute()
	{
		float3 alignmentMove = new float3();

		for (int i = 0; i < NumberOfItems; i++)
		{
			alignmentMove += ItemPositions[i];
		}
		alignmentMove /= NumberOfItems;
		
		Result[0] = math.cross(Forward, alignmentMove);
	}
}
