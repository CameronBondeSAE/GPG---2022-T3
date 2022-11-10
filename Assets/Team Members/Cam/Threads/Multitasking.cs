using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class Multitasking : MonoBehaviour
{
	public int number = 0;

	public float total = 0;

	public Stopwatch stopwatch;
	
    // Start is called before the first frame update
    void Start()
    {
	    stopwatch = new Stopwatch();
    }

    private void Update()
    {
	    if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
	    {
		    // for (int i = 0; i < 25; i++)
		    // {
			    // Thread thread = new Thread(DoThings);
			    // thread.Start();
		    // }
		    
		    for (int i = 0; i < 5; i++)
		    {
			    CamJob camJob = new CamJob();
			    camJob.number = number;
			    JobHandle jobHandle1 = camJob.Schedule();
			    //jobHandle1.Complete(); // BAD! Blocks main thread, won't run any faster

		    }
	    }
    }

    public void DoThings()
    {
	    Debug.Log("Start");
	    stopwatch.Start();
	    for (int i = 1; i < number; i++)
	    {
		    total += Mathf.Sin(i) * 1.44f / Mathf.Atan(i);
	    }
	    Debug.Log(total + " : Time taken = "+stopwatch.ElapsedMilliseconds);
    }
}
