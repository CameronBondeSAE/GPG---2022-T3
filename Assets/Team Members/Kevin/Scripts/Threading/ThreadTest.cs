using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

namespace Kevin
{
    
    public class ThreadTest : MonoBehaviour
    {
        public int badExampleNumber;
        public int goodExampleNumber;
        
        [Button]
        public void BadExample()
        {
            Debug.Log("Bad Example");
            for (int i = 0; i < badExampleNumber; i++)
            {
                Jobs jobs = new Jobs();
                JobHandle jobHandle1 = jobs.Schedule();
                jobHandle1.Complete(); // BAD! Blocks main thread, won't run any faster

            }
        }
        
        [Button]
        public void GoodExample()
        {
            NativeArray<JobHandle> handles = new NativeArray<JobHandle>(goodExampleNumber, Allocator.Temp);
            
            Debug.Log("Good Example");
            Profiler.BeginSample("GoodExample");
            for (int i = 0; i < goodExampleNumber; i++)
            {
                Jobs jobs = new Jobs();
                JobHandle jobHandle1 = jobs.Schedule();

                handles[i] = jobHandle1;
            }
            

            JobHandle.CompleteAll(handles); // Blocks this thread

            handles.Dispose();
            Profiler.EndSample();
        }
        
    }
}

