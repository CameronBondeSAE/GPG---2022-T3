using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;



namespace Kevin
{
    public struct Jobs : IJob
    {
        public void Execute()
        {
            float testTotal = 0;

            for (int i = 0; i < 500000; i++)
            {
                testTotal += Mathf.PerlinNoise(i * 25, i * 50);
            }
            Debug.Log(testTotal);
            Debug.Log("~~Completed~~");
        }
    }
}
