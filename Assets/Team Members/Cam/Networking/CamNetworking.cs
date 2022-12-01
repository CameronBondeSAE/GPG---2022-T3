using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CamNetworking : NetworkBehaviour
{
	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		
		Debug.Log("OnNetworkSpawn");
	}

	private void Update()
	{
		if (IsServer && Random.Range(0,150) == 0)
		{
			DoThingClientRpc(Random.Range(1f, 5f));
		}
	}

	[ClientRpc]
    public void DoThingClientRpc(float range)
    {
	    Debug.Log("Do thing");
	    transform.localScale = Vector3.one * range;
    }
}
