using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Luke
{
public class GameManager : NetworkBehaviour
{
	public static GameManager singleton;
	
	public event Action OnGameStart;
	public event Action OnGameEnd;

	public int playersAlive;
	public void InvokeOnGameStart()
	{
		if (IsServer)
		{
			InvokeOnGameStartClientRPC();
		}
	}

	[ClientRpc]
	private void InvokeOnGameStartClientRPC()
	{
		Debug.Log("Game Started!!!");
		OnGameStart?.Invoke();
	}

	public void InvokeOnGameEnd()
	{
		if (IsServer)
		{
			InvokeOnGameEndClientRPC();
		}
	}
	
	[ClientRpc]
	private void InvokeOnGameEndClientRPC()
	{
		OnGameEnd?.Invoke();
	}

	void Awake()
	{
		singleton = this;
	}
}
}
