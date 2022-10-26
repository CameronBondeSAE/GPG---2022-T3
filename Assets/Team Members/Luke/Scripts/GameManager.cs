using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Luke
{
public class GameManager : NetworkBehaviour
{
	public static GameManager singleton;

    public event Action OnGameStart;
	public event Action OnGameEnd;

    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private GameObject cameraPrefab;

    /*public GameObject localSpawnedPlayer;
    public Transform localPlayerTransform;*/
    
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

	[ClientRpc]
	private void SetCameraTargetClientRpc()
	{
		cameraPrefab.GetComponent<CameraTracker>().target = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
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

    private void SubscribeToSceneEvent()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnAvatars;
    }

    private void SpawnAvatars(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (!IsServer) return;
        foreach (KeyValuePair<ulong, NetworkClient> client in NetworkManager.Singleton.ConnectedClients)
        {
            GameObject avatar = Instantiate(avatarPrefab);
            avatar.GetComponent<NetworkObject>().SpawnWithOwnership(client.Value.ClientId);
            client.Value.PlayerObject.GetComponent<ClientEntity>()
                .AssignAvatarClientRpc(avatar.GetComponent<NetworkObject>().NetworkObjectId);
        }
        SetCameraTargetClientRpc();
    }
    
    void Awake()
	{
		singleton = this;
	}

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SubscribeToSceneEvent;
    }
}
}
