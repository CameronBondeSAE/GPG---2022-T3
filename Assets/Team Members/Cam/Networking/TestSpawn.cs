using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestSpawn : NetworkBehaviour
{
	public GameObject prefab;

    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();

	    if (IsServer)
	    {
		    var o = Instantiate(prefab);
		    foreach (NetworkObject componentsInChild in o.GetComponentsInChildren<NetworkObject>())
		    {
			    componentsInChild.Spawn();
		    }
	    }
    }
}
