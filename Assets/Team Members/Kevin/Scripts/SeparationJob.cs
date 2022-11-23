using System.Collections;
using System.Collections.Generic;
using Anthill.Effects;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct SeparationJob : IJob
{
    [ReadOnly]
    public NativeArray<float3> NeighbourPosition;
    
    public float3 MyPosition;
    public int NumberOfNeighbours;
    private float3 Direction;
    public float3 NormalizedDirection;
    public void Execute()
    {
        float3 separationMove = new float3();
        /*float normalizedDirectionX;
        float normalizedDirectionY;
        float normalizedDirectionZ;*/
        
        for (int i = 0; i < NumberOfNeighbours; i++)
        {
            separationMove += NeighbourPosition[i];
        }
        
        separationMove /= NumberOfNeighbours;
        Direction = (MyPosition - separationMove);
        
        /*float vectorLength = math.length(Direction);
        
        NormalizedDirection = Direction.xyz / vectorLength;*/
        
        NormalizedDirection = math.normalizesafe(Direction);
        /*normalizedDirectionX = direction.x / vectorLength;
        normalizedDirectionY = direction.y / vectorLength;
        normalizedDirectionZ = direction.z / vectorLength;

        normalizedDirection = new float3(normalizedDirectionX, normalizedDirectionY, normalizedDirectionZ);*/
    }
}
