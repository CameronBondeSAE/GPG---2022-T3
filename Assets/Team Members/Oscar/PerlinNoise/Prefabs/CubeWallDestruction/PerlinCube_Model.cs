using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using Unity.Netcode;
using UnityEngine;

public class PerlinCube_Model : NetworkBehaviour
{
    public event Action wallDestruction;

    private PerlinCube_View view;


    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();
	    
	    if (!IsServer) return;
	    view = GetComponentInChildren<PerlinCube_View>();
	    GetComponent<Health>().YouDied += DestroyTheWall;
    }

    void DestroyTheWall(GameObject go)
    {
	    GetComponent<Collider>().enabled = false;
	    DestroyTheWallClientRpc();
	    if (!NetworkManager.Singleton.IsServer) return;
	    GridGenerator.singleton.Scan();
	    wallDestruction?.Invoke();
	    if(IsServer) StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
	    yield return new WaitForSeconds(view.firstDelay + view.secondDelay);
	    Destroy(gameObject);
    }

    //destroy the wall
    [ClientRpc]
    public void DestroyTheWallClientRpc()
    {
	    // TODO: Only server
	    // World changed. Pathfinding update world
		// HACK
	    GetComponent<Collider>().enabled = false;
	    
    }
}
