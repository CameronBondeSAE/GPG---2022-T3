using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kevin;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Luke
{
public class GameManagerTemp : NetworkBehaviour
{
	public static GameManagerTemp singleton;

    public event Action OnGameStart;
	public event Action OnGameEnd;

    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] public GameObject cameraPrefab;
    [SerializeField] public CinemachineVirtualCamera virtualCameraOne;
    [SerializeField] public CinemachineVirtualCamera virtualCameraTwo;
    [SerializeField] private GameObject playerNamePrefab;

    [SerializeField] private GameObject countdownTimer;
    
    //In Game Counts
	public int playersAlive;
	public int playersInGame;
	public int amountOfResources;

	public bool zoomedIn;
	
	public NetworkManager myLocalClient;
	public void InvokeOnGameStart()
	{
		if (IsServer)
		{
			GameObject go = Instantiate(countdownTimer);
			go.GetComponent<NetworkObject>().Spawn();
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
		Transform playerTransform = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
		virtualCameraOne.Follow = playerTransform;
		virtualCameraOne.LookAt = playerTransform;

		virtualCameraTwo.Follow = playerTransform;
		virtualCameraTwo.LookAt = playerTransform;
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
		Debug.Log("Times Up!");
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
            playersAlive++;
            playersInGame++;
            /*avatar.GetComponent<PlayerNameTracker>().playerName.text =
	            client.Value.PlayerObject.GetComponent<ClientInfo>().ClientName.Value.ToString();*/
        }
        SetCameraTargetClientRpc();
        //SetPlayerNameClientRpc();
    }

    /*[ClientRpc]
    private void SetPlayerNameClientRpc()
    {
	    
    }*/
    
    void Awake()
	{
		singleton = this;
    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SubscribeToSceneEvent;
        // SpawnCamera(); // Move this to when the level is loaded.
    }

    private void SpawnCamera()
    {
	    GameObject go = Instantiate(cameraPrefab);
	    CinemachineVirtualCamera[] vCameras = go.GetComponentsInChildren<CinemachineVirtualCamera>();
	    virtualCameraOne = vCameras[0];
	    virtualCameraTwo = vCameras[1];
    }

    public void LevelFinishedLoading()
    {
	    
    }
}
//Things to add
//Game end logic
//Adding player names onto player prefabs
//Total time alive as a human
//Total resources in the world
//
}
