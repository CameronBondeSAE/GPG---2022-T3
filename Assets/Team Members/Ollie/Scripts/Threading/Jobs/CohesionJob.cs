using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Ollie
{
    [BurstCompile]
    public struct CohesionJob : IJob
    {
        public NativeArray<float3> averagePosition;
        [ReadOnly] public NativeArray<float3> neighbourPositions;
        [ReadOnly] public NativeArray<float3> myPos;
        public NativeArray<float3> direction;
        [ReadOnly] public NativeArray<int> neighboursCount;

        public void Execute()
        {
            for (int i = 0; i < neighboursCount[0]; i++)
            {
                averagePosition[0] += neighbourPositions[i];
            }

            averagePosition[0] /= neighboursCount[0];

            direction[0] = averagePosition[0] - myPos[0];

            //Debug.Log("job complete?");
        }
    }
}
