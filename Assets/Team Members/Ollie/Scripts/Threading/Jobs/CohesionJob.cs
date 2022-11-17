using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Ollie
{
    public struct CohesionJob : IJobParallelFor
    {
        public NativeArray<float3> averagePosition;
        [ReadOnly] public NativeArray<float3> neighbourPositions;
        [ReadOnly] public NativeArray<float3> myPos;
        public NativeArray<float3> direction;
        [ReadOnly] public NativeArray<int> neighboursCount;

        public void Execute(int index)
        {
            for (int i = 0; i < neighboursCount[0]; i++)
            {
                averagePosition[0] += neighbourPositions[i];
            }

            averagePosition[0] /= neighboursCount[0];

            direction[0] = averagePosition[0] - myPos[0];
        }
    }
}
