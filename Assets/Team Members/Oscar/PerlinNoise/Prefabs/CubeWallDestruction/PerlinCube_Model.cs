using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinCube_Model : MonoBehaviour
{
    public event Action wallDestruction;
    
    // Start is called before the first frame update
    void Awake()
    {
	    GetComponent<Health>().YouDied += DestroyTheWall;
    }

    //destroy the wall
    public void DestroyTheWall()
    {
        wallDestruction?.Invoke();
    }
}
