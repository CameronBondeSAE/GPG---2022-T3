using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kevin;
using Lloyd;
using Oscar;
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
    [SerializeField] public GameObject cameraPrefab;
    [SerializeField] public GameObject virtualCameraOne;
    [SerializeField] public GameObject virtualCameraTwo;
    [SerializeField] private GameObject playerNamePrefab;

    [SerializeField] private GameObject countdownTimer;

    private ILevelGenerate levelGenerator;

    public ILevelGenerate LevelGenerator
    {
	    get;
	    set;
    }
    
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
		GameObject go = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer;
		if (go == null) return;
		virtualCameraOne.GetComponent<CinemachineVirtualCamera>().Follow = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
		virtualCameraOne.GetComponent<CinemachineVirtualCamera>().LookAt = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;

		virtualCameraTwo.GetComponent<CinemachineVirtualCamera>().Follow = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
		virtualCameraTwo.GetComponent<CinemachineVirtualCamera>().LookAt = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
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
		NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SetupScene;
	}

    private void SetupScene(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
	    if (!IsServer) return;
	    
	    // TODO: Wait for level generation callback to be sure it's finished.
	    LevelGenerator.SpawnPerlin();
	    LevelGenerator.SpawnItems();
	    LevelGenerator.SpawnExplosives();
	    LevelGenerator.SpawnBases();
	    LevelGenerator.SpawnAI();
	    LevelGenerator.SpawnBorder();
	    
        foreach (KeyValuePair<ulong, NetworkClient> client in NetworkManager.Singleton.ConnectedClients)
        {
	        // CAM HACK: Find base spawn and spawnpoints
	        Transform spawnTransform = null;
	        foreach (HQ hq in FindObjectsOfType<HQ>())
	        {
		        if (hq.type == HQ.HQType.Humans)
		        {
			        spawnTransform = hq.GetComponentInChildren<SpawnPoint>().transform;
		        }
	        }


	        if (spawnTransform != null) // No Spawns found
	        {
		        GameObject avatar = Instantiate(avatarPrefab, spawnTransform.position, spawnTransform.rotation);
		        avatar.GetComponent<NetworkObject>().SpawnWithOwnership(client.Value.ClientId);
		        client.Value.PlayerObject.GetComponent<ClientEntity>()
			        .AssignAvatarClientRpc(avatar.GetComponent<NetworkObject>().NetworkObjectId);
	        }

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
    }

    public void SpawnPerlinFinished()
    {
	    
    }

    public void SpawnBorderFinished()
    {
	    
    }

    public void SpawnAIFinished()
    {
	    
    }

    public void SpawnItemsFinished()
    {
	    
    }

    public void SpawnExplosivesFinished()
    {
	    
    }

    public void SpawnBasesFinished()
    {

    }

    public void LevelFinishedLoading()
    {
	    //This will be removed, but I don't want to cause errors on github
    }
}
//Things to add
//Game end logic
//Adding player names onto player prefabs
//Total time alive as a human
//Total resources in the world
//
}
