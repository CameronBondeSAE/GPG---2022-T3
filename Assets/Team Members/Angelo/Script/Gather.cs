using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gather : MonoBehaviour
{
    public delegate void GatherAction();
    public static event GatherAction UpdateSwarm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(UpdateSwarm != null)
            {
                UpdateSwarm();
            }
        }
    }
}
