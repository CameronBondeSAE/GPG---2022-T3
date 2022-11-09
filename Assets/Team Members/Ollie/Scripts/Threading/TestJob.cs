using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Profiling;

namespace Ollie
{
    public class TestJob : MonoBehaviour
    {
        private JobHandle jobHandle1;
        private NativeArray<JobHandle> handles;
        public int basicNumber;
        public int betterNumber;

        [Button]
        private void BasicJobTest()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            for (int i = 0; i < basicNumber; i++)
            {
                Jobs job = new Jobs();
                jobHandle1 = job.Schedule();
                jobHandle1.Complete();
            }
            
            UnityEngine.Debug.Log("Time taken = " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Stop();
        }
        
        [Button]
        private void BetterJobTest()
        {
            //Profiler.BeginSample("Better Job Test Profile");
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            handles = new NativeArray<JobHandle>(betterNumber, Allocator.Temp);
            for (int i = 0; i < handles.Length; i++)
            {
                Jobs job = new Jobs();
                jobHandle1 = job.Schedule();
                handles[i] = jobHandle1;
            }
            JobHandle.CompleteAll(handles);
            handles.Dispose();
            
            UnityEngine.Debug.Log("Time taken = " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Stop();
            
            
            //Profiler.EndSample();
        }
    }
}
