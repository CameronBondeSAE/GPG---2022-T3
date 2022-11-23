using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PerlinCube_Model : MonoBehaviour
{
    public event Action wallDestruction;
    
    // Start is called before the first frame update
    void Awake()
    {
	    GetComponent<Health>().YouDied += DestroyTheWallClientRpc;
    }

    //destroy the wall
    [ClientRpc]
    public void DestroyTheWallClientRpc()
    {
        wallDestruction?.Invoke();
    }
}
