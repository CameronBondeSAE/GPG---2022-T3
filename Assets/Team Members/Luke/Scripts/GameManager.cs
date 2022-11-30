using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kevin;
using Lloyd;
using Ollie;
using Oscar;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Luke
{
public class GameManager : NetworkBehaviour
{
	public static GameManager singleton;
	public Health health;
	
    public event Action OnGameStart;
	public event Action OnGameEnd;
	public event Action OnGameWaveTimer;
	
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] public GameObject virtualCameraOne;
    [SerializeField] public GameObject virtualCameraTwo;
    [SerializeField] private GameObject playerNamePrefab;

    [SerializeField] private GameObject countdownTimer;
    [SerializeField] private GameObject aiPrefab;
    
    private ILevelGenerate levelGenerator;

    public ILevelGenerate LevelGenerator
    {
	    get => levelGenerator;
	    set => levelGenerator = value;
    }
    
    //In Game Counts
	public int playersAlive;
	public int playersInGame;
	public int amountOfResources;
	public int amountOfAIInGame;
	public int maxAI;
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

	public void InvokeOnGameWaveTimer()
	{
		if (IsServer)
		{
			/*for (int i = amountOfAIInGame; i < maxAI; i++)
			{
				//should i change the ai spawner to be in the game manager or leave it for each map generation?
				//The position needs to be pulled from the original spawner function
				NetworkInstantiate(aiPrefab,new Vector3(1,1,1),quaternion.identity);
			}*/
			InvokeOnGameWaveTimerClientRPC();
		}
	}

	[ClientRpc]
	private void InvokeOnGameWaveTimerClientRPC()
	{
		Debug.Log("Wave Spawned!!!");
		OnGameWaveTimer?.Invoke();
	}

	//replace this spawn function with the spawn function called at level load?
	private void SpawnAI()
	{
		/*for (int i = amountOfAIInGame; i < maxAI; i++)
		{
			NetworkInstantiate(aiPrefab,new Vector3(1,1,1),quaternion.identity);
		}*/
	}

	private void AIDied()
	{
		amountOfAIInGame--;
	}
	
	[ClientRpc]
	private void SetCameraTargetClientRpc()
	{
		//need to disable cameraTwo, otherwise it defaults to zoomed out view
		//which is necessary to see the zoomed out level preview in lobby
		virtualCameraTwo.SetActive(false);
		
		GameObject go = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer;
		if (go == null) return;
		virtualCameraOne.GetComponent<CinemachineVirtualCamera>().Follow = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
		virtualCameraOne.GetComponent<CinemachineVirtualCamera>().LookAt = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;

		
		// virtualCameraTwo.GetComponent<CinemachineVirtualCamera>().Follow = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
		// virtualCameraTwo.GetComponent<CinemachineVirtualCamera>().LookAt = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.transform;
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
		//NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SetupScene;
		
		
		//OLLIE HACK: Subscribed SetupScene to the LobbyUIManager instead of the above, commented out line
		//Means SetupScene only occurs when the Lobby's Start Game button is pressed
		//Allows Lobby to load scenes in, call their Perlin spawn so level preview can exist
		//On Start Game, lobby unloads everything but Base scene, then loads the new scene FULLY
		//then SetupScene runs
		LobbyUIManager.LobbyGameStartEvent += SetupScene;
	}

    //private void SetupScene(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    private void SetupScene()
    {
	    //Ollie Hack: Enabled this camera in base scene by default, so the level preview can be seen
	    //sets back to false when level actually starts
	    virtualCameraTwo.SetActive(false);
	    if (!IsServer) return;

	    if (levelGenerator == null)
	    {
		    Debug.Log("No Level");
		    return;
	    }
	    
	    // TODO: Wait for level generation callback to be sure it's finished.
	    levelGenerator.SpawnPerlinClientRpc();
	    levelGenerator.SpawnItemsClientRpc();
	    levelGenerator.SpawnExplosivesClientRpc();
	    levelGenerator.SpawnBasesClientRpc();
	    levelGenerator.SpawnAIClientRpc();
	    levelGenerator.SpawnBorderClientRpc();
	    
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
	    //health.YouDied += AIDied;
    }

    public void NetworkInstantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
	    if (!IsServer) return;
	    if (prefab.GetComponent<NetworkObject>() == null) return;
	    GameObject go = Instantiate(prefab, position, rotation);
	    go.GetComponent<NetworkObject>().Spawn();
    }

    public void NetworkInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
	    if (!IsServer) return;
	    if (prefab.GetComponent<NetworkObject>() == null) return;
	    GameObject go = Instantiate(prefab, position, rotation);
	    NetworkObject no = go.GetComponent<NetworkObject>();
	    no.Spawn();
	    no.TrySetParent(parent);
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
