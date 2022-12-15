using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alex;
using Cinemachine;
using DG.Tweening;
using Kevin;
using Lloyd;
using Ollie;
using Oscar;
using ParadoxNotion;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Luke
{
public class GameManager : NetworkBehaviour
{
	public static GameManager singleton;
	public Health health;
	public SpawnManager spawnManager;
	public GridGenerator gridGenerator;
    public event Action OnGameStart;
	public event Action OnGameEnd;
	public event Action OnGameWaveTimer;
	
	public event Action GameHasStartedEvent;
	
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] public CinemachineBrain cameraBrain;
    [SerializeField] public GameObject virtualCameraOne;
    [SerializeField] public GameObject virtualCameraTwo;
    [SerializeField] private GameObject playerNamePrefab;

    [SerializeField] private GameObject countdownTimer;
    [SerializeField] private GameObject aiPrefab;

    public List<GameObject> hqSpawnPointObject;
    public List<GameObject> flamethrowerSpawnPointObject;
    public List<GameObject> waterCannonSpawnPointObject;
    
    private ILevelGenerate levelGenerator;

    public ILevelGenerate LevelGenerator
    {
	    get => levelGenerator;
	    set => levelGenerator = value;
    }
    
    //In Game Counts
	public int playersAlive;
	public int playersInGame;
	public int targetEndResources;
	public int amountOfResources;
	public int amountOfAIInGame;
	public int maxAI;
	public bool zoomedIn;

	public ulong[,] entityClientIdAndPlayerNetworkObjectIdPairs;
	
	public NetworkManager myLocalClient;
	[SerializeField]
	private float respawnDelay = 3f;

	public void InvokeOnGameStart()
	{
		if (IsServer)
		{
			//GameObject go = NetworkInstantiate(countdownTimer, countdownTimer.transform.position, countdownTimer.transform.rotation);
			InvokeOnGameStartClientRPC();
		}
	}

	[ClientRpc]
	private void InvokeOnGameStartClientRPC()
	{
		OnGameStart?.Invoke();
	}

	public void InvokeOnGameWaveTimer()
	{
		if (IsServer)
		{
			if (levelGenerator == null) return;
			levelGenerator.SpawnAI();
			InvokeOnGameWaveTimerClientRPC();
		}
	}

	[ClientRpc]
	private void InvokeOnGameWaveTimerClientRPC()
	{
		OnGameWaveTimer?.Invoke();
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
		LobbyUIManager.singleton.LobbyGameStartEvent += SetupScene;
	}

    //private void SetupScene(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    private void SetupScene()
    {
	    playersInGame = 0;
	    GameHasStartedEvent += InvokeOnGameStart;
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
	    
	    levelGenerator.SpawnPerlin(); // not actually a client RPC
	    levelGenerator.SpawnItems();
	    levelGenerator.SpawnExplosives();
	    levelGenerator.SpawnBorderClientRpc();
	    levelGenerator.SpawnBases();
	    // Pathfinding
	    gridGenerator.Scan();

	    entityClientIdAndPlayerNetworkObjectIdPairs = new ulong[NetworkManager.Singleton.ConnectedClients.Count,2];
	    
	    for (int i=0; i< NetworkManager.Singleton.ConnectedClients.Count; i++)
        {
	        // CAM HACK: Find base spawn and spawnpoints
	        Transform spawnTransform = null;
	        foreach (HQ hq in FindObjectsOfType<HQ>())
	        {
		        if (hq.type == HQ.HQType.Humans)
		        {
			        SpawnPoint[] spawnPoints = hq.GetComponentsInChildren<SpawnPoint>();
			        spawnTransform = spawnPoints[i % spawnPoints.Length].transform;
		        }
	        }
	        
	        if (spawnTransform != null) // No Spawns found
	        {
		        GameObject avatar = Instantiate(avatarPrefab, spawnTransform.position, spawnTransform.rotation);
		        NetworkObject no = avatar.GetComponent<NetworkObject>();
		        avatar.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.ConnectedClientsList[i].ClientId);
                NetworkObject clientEntity = NetworkManager.Singleton.ConnectedClients.ElementAt(i).Value.PlayerObject;
                clientEntity.GetComponent<ClientEntity>().ControlledPlayer = avatar;
                avatar.GetComponent<Avatar>().SetName(clientEntity.GetComponent<ClientInfo>().ClientName
	                .Value.ToString());
                entityClientIdAndPlayerNetworkObjectIdPairs[i,0] = clientEntity.NetworkObjectId;
		        entityClientIdAndPlayerNetworkObjectIdPairs[i,1] = no.NetworkObjectId;
		        playersAlive++;
		        playersInGame++;
		        avatar.GetComponent<Health>().YouDied += PlayerDied;
	        }
        }
	    
	    void PlayerDied(GameObject go)
	    {
		    if (go.GetComponent<Avatar>() == null) return;

		    StartCoroutine(RespawnPlayer(go));
	    }

	    IEnumerator RespawnPlayer(GameObject go)
	    {
		    PlayerController pc = NetworkManager.Singleton
			    .ConnectedClients[go.GetComponent<NetworkObject>().OwnerClientId].PlayerObject
			    .GetComponent<PlayerController>();
		    pc.DisableControlsClientRpc();

		    Avatar avatar = go.GetComponent<Avatar>();
		    Interact interact = go.GetComponent<Interact>();
		    
		    yield return new WaitUntil(() => interact.storedItems <= 0);
		    yield return new WaitForSeconds(respawnDelay);

		    gridGenerator.Scan();
		    
		    Checkpoint checkpoint = null;
		    
		    foreach (HQ hq in FindObjectsOfType<HQ>())
		    {
			    if (hq.type == HQ.HQType.Humans)
			    {
				    checkpoint = hq.GetComponentInChildren<Checkpoint>();
				    SpawnPoint[] spawnPoints = hq.GetComponentsInChildren<SpawnPoint>();
				    Transform avatarTransform = avatar.transform;
				    Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
				    avatarTransform.position = spawn.position;
				    avatarTransform.rotation = spawn.rotation;
				    break;
			    }
		    }

		    avatar.ToggleMeshRenderersClientRpc(true);
		    avatar.ActivateHatRandomiser();
		    if (checkpoint != null) checkpoint.PlayerDied();
		    Health health = go.GetComponent<Health>();
		    health.ChangeHP(100000);
		    health.isAlive = true;
		    pc.EnableControlsClientRpc();
	    }

	    for (int i = 0; i < entityClientIdAndPlayerNetworkObjectIdPairs.GetLength(0); i++)
	    {
		    RegisterPlayerAvatarsClientRpc(entityClientIdAndPlayerNetworkObjectIdPairs[i,0], entityClientIdAndPlayerNetworkObjectIdPairs[i,1]);
	    }

	    SetCameraTargetClientRpc();
	    SpawnPointPos();
	    FlamethrowerSpawnPointPos();
	    WaterCannonSpawnPointPos();
	    spawnManager.SpawnFlameThrowers();
	    spawnManager.SpawnWaterCannon();
	    levelGenerator.SpawnAI();
        spawnManager.SpawnBossAI();
        targetEndResources = playersInGame * targetEndResources; 
        GameHasStartedEvent?.Invoke();
        GameObject go = NetworkInstantiate(countdownTimer, countdownTimer.transform.position, countdownTimer.transform.rotation);
    }

    private void SpawnPointPos()
    {
	    foreach (SpawnPoint spawnPoint in FindObjectsOfType<SpawnPoint>())
	    { 
		    if (spawnPoint.GetComponentInParent<HQ>().type == HQ.HQType.Aliens)
		    {
			    hqSpawnPointObject.Add(spawnPoint.gameObject); 
		    }
	    }
    }

    private void FlamethrowerSpawnPointPos()
    {
	    foreach (SpawnPoint spawnPoint in FindObjectsOfType<SpawnPoint>())
	    {
		    if (spawnPoint.GetComponentInParent<HQ>().type == HQ.HQType.Humans)
		    {
				    flamethrowerSpawnPointObject.Add(spawnPoint.gameObject);
		    }
	    }
	    
    }
    
    private void WaterCannonSpawnPointPos()
    {
	    foreach (WaterSpawnPoint waterSpawnPoint in FindObjectsOfType<WaterSpawnPoint>())
	    {
		    if (waterSpawnPoint.GetComponentInParent<HQ>().type == HQ.HQType.Humans)
		    {
			    waterCannonSpawnPointObject.Add(waterSpawnPoint.gameObject);
		    }
	    }
	    
    }
    
    

    [ClientRpc]
    private void RegisterPlayerAvatarsClientRpc(ulong clientNetworkId,ulong playerNetworkId)
    {
	    NetworkManager.Singleton.SpawnManager.SpawnedObjects[clientNetworkId].GetComponent<ClientEntity>()
			    .ControlledPlayer = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerNetworkId].gameObject;
    }
    void Awake()
	{
		singleton = this;
	}
    
    private void Start()
    {
	    // Set max Tweeners to 4000 and max Sequences to 100
	    DOTween.SetTweensCapacity(4500, 100);
	    
	    NetworkManager.Singleton.OnServerStarted += SubscribeToSceneEvent;
	    //health.YouDied += AIDied;
    }

    public GameObject NetworkInstantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
	    if (!IsServer) return null;
	    if (prefab.GetComponent<NetworkObject>() == null) return null;
	    GameObject go = Instantiate(prefab, position, rotation);
	    go.GetComponent<NetworkObject>().Spawn();
	    return go;
    }

    public GameObject NetworkInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
	    if (!IsServer) return null;
	    if (prefab.GetComponent<NetworkObject>() == null) return null;
	    GameObject go = Instantiate(prefab, position, rotation);
	    NetworkObject no = go.GetComponent<NetworkObject>();
	    no.Spawn();
	    no.TrySetParent(parent);
	    return go;
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
}
