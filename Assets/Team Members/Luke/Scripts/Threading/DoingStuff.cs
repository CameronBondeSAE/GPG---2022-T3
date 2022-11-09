using System.Collections;
using System.Collections.Generic;
using Luke;
using UnityEngine;
using Unity.Jobs;

public class DoingStuff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10000; i++)
        {
            LukeJob job = new LukeJob();
            job.point = new Vector3(i*i, 2 * i, 0);
            job.Schedule();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
