using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
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
	    // TODO: Only server
	    // World changed. Pathfinding update world
		// HACK
	    GetComponent<Collider>().enabled = false;
	    if(NetworkManager.Singleton.IsServer) GridGenerator.singleton.Scan();

	    
	    
        wallDestruction?.Invoke();
    }
}
