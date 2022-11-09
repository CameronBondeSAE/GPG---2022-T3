using System;
using System.Collections;
using System.Collections.Generic;
using ParadoxNotion.Serialization.FullSerializer;
using Unity.Jobs;
using UnityEngine;

namespace Ollie
{
    public struct Jobs : IJob
    {
        public void Execute()
        {
            float answer = 0;

            for (int i = 0; i < 500000; i++)
            {
                answer += Mathf.Sin(42) * Mathf.Cos(69);
            }
            
            Debug.Log("Answer = " + answer);
        }
    }
}
