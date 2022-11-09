using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace Luke
{
    public struct LukeJob : IJob
    {
        public Vector3 point;
        
        public void Execute()
        {
            Debug.Log(point.normalized);
        }
    }
}