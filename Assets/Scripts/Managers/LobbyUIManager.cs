using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;



namespace Ollie
{
    [Serializable]
    public class Level
    {
        public Object level;
        public string levelNameOnUI;
    }

    public class LobbyUIManager : NetworkBehaviour
    {
	    public static LobbyUIManager singleton;
	    
        [Header("Testing")]
        public bool autoHost;

        public bool autoLoadLevel;
        public bool spawnPlayerOnAwake;
        public string sceneToLoad;

        [Header("Level Setup")]
        public List<Level> levels;

        public GameObject levelHolder;
        public GameObject levelButtonPrefab;

        [Header("Lobby UI Setup")]
        public GameObject lobbyUICanvas;

        public TMP_Text clientUI;
        public Button startButton;
        public Button clientLockButton;
        public Button lobbyButton;
        public TMP_InputField playerNameInputField;
        public TMP_Text levelSelectedDisplayText;
        public GameObject levelDisplayUI;
        public GameObject waitForHostBanner;
        public GameObject playerPanel;
        public GameObject clientLobbyUIPrefab;

        [Header("IP Canvas Setup")]
        public GameObject ipAddressCanvas;

        public GameObject endGameUI;

        public TMP_InputField serverIPInputField;
        
        [Header("Hack for now/Ignore")]
        //public GameObject playerPrefab;

        //public GameObject lobbyCam;
        //public GameObject directionalLight;
        //bool              inGame = false;

        ulong         myLocalClientId;
        NetworkObject myLocalClient;
        string        clientName;

        
        public event Action LobbyGameStartEvent;
        public GameManager gameManager;


        #region Lobby Specific Stuff

        private void Awake()
        {
	        singleton = this;

            ipAddressCanvas.SetActive(true);
            lobbyUICanvas.SetActive(false);
            gameManager = GameManager.singleton;
            
        }
        
        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;

            serverIPInputField.text = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;

            if (autoHost)
            {
                //Host Game
            }

            if (autoLoadLevel)
            {
                //Start Game
            }
        }
        
        public void HostGame()
        {
            NetworkManager.Singleton.StartHost();
            

            lobbyUICanvas.SetActive(true);
            startButton.gameObject.SetActive(false);
            ipAddressCanvas.SetActive(false);
        }

        public void GoToLevelSelection()
        {
            clientLockButton.gameObject.SetActive(false);
            foreach (Level level in levels)
            {
                GameObject levelButton = Instantiate(levelButtonPrefab, levelHolder.transform);
                levelButton.GetComponentInChildren<TMP_Text>().text = level.levelNameOnUI;
                levelButton.GetComponent<LevelButton>().myLevel = level.level.name;
            }
        }
        
        public void JoinGame()
        {
            NetworkManager.Singleton.StartClient();
            SetUpClientUI();
        }

        public void JoinCamsHouse()
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = "121.200.8.114";
            NetworkManager.Singleton.StartClient();
            SetUpClientUI();
        }
        
        private void SetUpClientUI()
        {
            startButton.gameObject.SetActive(false);
            clientLockButton.gameObject.SetActive(false);
            lobbyUICanvas.SetActive(true);
            ipAddressCanvas.SetActive(false);
            levelDisplayUI.SetActive(false);
            endGameUI.SetActive(false);
        }
        
        public void OnNewServerIPAddress()
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = serverIPInputField.text;
        }

        private void OnClientJoin(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer || IsOwner)
            {
                NetworkClient client;
                if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out client))
                {
                    ClientInfo clientInfo = client.PlayerObject.GetComponent<ClientInfo>();
                    clientInfo.Init((ulong) NetworkManager.Singleton.ConnectedClients.Count);

                    GameObject uiRef = Instantiate(clientLobbyUIPrefab, playerPanel.transform);
                    clientInfo.lobbyUIRef = uiRef;
                    uiRef.GetComponent<TMP_Text>().text = clientInfo.ClientName.Value.ToString();
                }
                HandleLocalClient(clientId);
            }
            //else RequestClientNamesLobbyUIServerRpc(clientId);
            else RequestClientUIUpdateServerRpc();

            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                myLocalClientId = clientId;
            }

        }

        //this allows the HOST to have a myLocalClient value
        void HandleLocalClient(ulong clientId)
        {
            NetworkClient temporaryClient;
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out temporaryClient))
            {
                NetworkObject playerObject = temporaryClient.PlayerObject;
                if (playerObject.IsLocalPlayer)
                {
                    myLocalClient = playerObject;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestClientUIUpdateServerRpc()
        {
            HandleClientNameChange();
        }

        private void HandleClientNameChange()
        {
            ClearLobbyNamesClientRpc();

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                SpawnClientLobbyUIClientRpc(client.PlayerObject.GetComponent<ClientInfo>().ClientName.Value.ToString());
            }
        }

        [ClientRpc]
        private void ClearLobbyNamesClientRpc()
        {
            foreach (Transform child in playerPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }

        [ClientRpc]
        private void SpawnClientLobbyUIClientRpc(string newName)
        {
            SpawnClientLobbyUI(newName);
        }

        private void SpawnClientLobbyUI(string clientName)
        {
            GameObject uiRef = Instantiate(clientLobbyUIPrefab, playerPanel.transform);
            uiRef.GetComponent<TMP_Text>().text = clientName;
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<ClientInfo>().lobbyUIRef = uiRef;
        }

        public void UpdateClientName()
        {
            if (IsServer)
            {
                if (myLocalClient != null)
                {
                    myLocalClient.GetComponent<ClientInfo>().ClientName.Value = playerNameInputField.text;
                    HandleClientNameChange();
                }
                else
                {
                    print("No local client found");
                }
            }
            else
            {
                RequestClientNameChangeServerRpc(myLocalClientId, playerNameInputField.text);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestClientNameChangeServerRpc(ulong clientId, string name)
        {
            NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<ClientInfo>().ClientName
                .Value = name;
            HandleClientNameChange();
        }

        public void CAMHACKQuickLoadTestLevelAndHost()
        {
	        autoHost = true;
	        HostGame();
	        LobbyUIManager.singleton.sceneToLoad = "OscarTinyTest";
	        LobbyUIManager.singleton.UpdateLevelSelectedText("OscarTinyTest");
	        GoToLevelSelection();
        }

        public void UpdateLevelSelectedText(string levelName)
        {
            levelSelectedDisplayText.text = levelName;
            startButton.gameObject.SetActive(false);
            //TODO: spawn perlin here somehow
            LobbyLevelPreview();
        }

        public void LobbyLevelPreview()
        {
	        if (SceneManager.sceneCount > 1)
	        {
		        for (int i = 0; i < SceneManager.sceneCount; i++)
		        {
			        Scene scene = SceneManager.GetSceneAt(i);
			        if (scene == SceneManager.GetActiveScene())
			        {
				        //SceneManager.UnloadSceneAsync(scene);
				        // UnloadSceneClientRpc(scene.name);
				        NetworkManager.Singleton.SceneManager.UnloadScene(scene);
			        }
		        }
	        }
	        NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
	        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
	        //yield return new WaitForSeconds(1f);
	        //GameManager.singleton.LevelGenerator.SpawnPerlinClientRpc();
        }
        
        private void OnLoadEventCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
        {
            if(IsServer) {startButton.gameObject.SetActive(true);}
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            UpdateScenesClientRpc(scenename);

            // CAM HACK: Quick test load
            if (!autoHost)
            {
	            if(GameManager.singleton.LevelGenerator != null) GameManager.singleton.LevelGenerator.SpawnPerlin();
            }
            else
            {
	            if (IsServer)
	            {
		            StartGame();
	            }
	            // if(IsServer)
					// StartCoroutine(HackWaitForABitForSomeReason());
            }
        }

        IEnumerator HackWaitForABitForSomeReason()
        {
	        yield return new WaitForSeconds(0.5f);
	        StartGame();
        }

        [ClientRpc]
        public void UnloadSceneClientRpc(string scenename)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(scenename));
        }

        [ClientRpc]
        public void UpdateScenesClientRpc(string scenename)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenename));
        }

        public void UnloadInactiveScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene == SceneManager.GetActiveScene())
                {
                    SceneManager.UnloadSceneAsync(scene);
                    //NetworkManager.Singleton.SceneManager.UnloadScene(scene);
                }
            }
        }
        
        #endregion

        //TODO: Rip this region into a Game Mode script, IF TIME PERMITS
        #region Game Mode Manager Stuff

        public void ExitToLobby()
        {
            if (SceneManager.sceneCount > 1)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (scene == SceneManager.GetActiveScene())
                    {
                        //SceneManager.UnloadSceneAsync(scene);
                        // UnloadSceneClientRpc(scene.name);
                        NetworkManager.Singleton.SceneManager.UnloadScene(scene);
                    }
                }
            }

            GameManager.singleton.flamethrowerSpawnPointObject.Clear();
            GameManager.singleton.waterCannonSpawnPointObject.Clear();
            GameManager.singleton.hqSpawnPointObject.Clear();
            GameManager.singleton.targetEndResources = 1000;

            startButton.gameObject.SetActive(false);
            levelSelectedDisplayText.text = "";
            lobbyUICanvas.SetActive(true);
            endGameUI.SetActive(false);
            ReturnToLobbyClientRpc();
            //unload scene
            //set base scene
            //same as switching level previews, but not adding in a level preview
        }

        [ClientRpc]
        void ReturnToLobbyClientRpc()
        {
            if (IsServer) return; //HACK: don't do it on the server because it already happens
            SetUpClientUI();
        }
        public void StartGame()
        {
            if (sceneToLoad == "")
            {
                print("You must select a level to load.");
                return;
            }

            NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManagerOnOnSceneEvent;
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
	            Scene scene = SceneManager.GetSceneAt(i);
	            if (scene == SceneManager.GetActiveScene())
	            {
		            //SceneManager.UnloadSceneAsync(scene);
		            // UnloadSceneClientRpc(scene.name);
		            NetworkManager.Singleton.SceneManager.UnloadScene(scene);
                }
            }

            GameManager.singleton.OnGameEnd += OnGameEnd;
        }

        private void OnGameEnd()
        {
            //disable active scene and enable base scene
            if (SceneManager.sceneCount > 1)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (scene == SceneManager.GetActiveScene())
                    {
                        NetworkManager.Singleton.SceneManager.UnloadScene(scene);
                    }
                }
            }
            
            //turn on end game ui for all clients
            OnGameEndClientRpc();
        }

        [ClientRpc]
        public void OnGameEndClientRpc()
        {
            lobbyUICanvas.SetActive(false);
            ipAddressCanvas.SetActive(false);
            endGameUI.SetActive(true);
        }
        
        
        //Sets the newly loaded scene to be the current active scene
        //So all future spawned objects appear in the new scene
        private void SetNewActiveScene(SceneEvent sceneEvent)
        {
	        if (sceneEvent.SceneEventType != SceneEventType.LoadEventCompleted) return;
	        
	        NetworkManager.Singleton.SceneManager.OnSceneEvent -= SetNewActiveScene;
            Scene scene = SceneManager.GetSceneByName(sceneEvent.SceneName);
            SceneManager.SetActiveScene(scene);
            
            UpdateScenesClientRpc(scene.name);

            StartCoroutine(LobbyGameStartDelayCoroutine());


            //BroadcastActiveSceneClientRpc(scenename);
        }

        public IEnumerator LobbyGameStartDelayCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            LobbyGameStartEvent?.Invoke();
        }

        //HACK: attempted to set the newly loaded scene as the active scene on the client
        //does not work - client tries to set active before it's finished loading
        //does work for host though
        [ClientRpc]
        private void BroadcastActiveSceneClientRpc(string sceneToActive)
        {
            Scene scene = (SceneManager.GetSceneByName(sceneToActive));
            SceneManager.SetActiveScene(scene);
        }
        
        private void SceneManagerOnOnSceneEvent(SceneEvent sceneEvent) 
        {
	        if (sceneEvent.SceneEventType == SceneEventType.UnloadComplete) // Preview needs to be fully unloaded before the same scene is attempted to be loaded again.
	        {
		        StartCoroutine(LoadScene());
	        }
	        if (sceneEvent.SceneEventType != SceneEventType.Load) return;
            NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManagerOnOnSceneEvent;
            Scene scene = sceneEvent.Scene;
            
            BroadcastLobbyUIStateClientRpc(true);
        }

        IEnumerator LoadScene()
        {
	        yield return 0;
	        try
	        {
		        NetworkManager.Singleton.SceneManager.OnSceneEvent += SetNewActiveScene;
		        NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
	        }
	        catch (Exception e)
	        {
		        Debug.LogException(e,this);
	        }
        }

        [ClientRpc]
        private void BroadcastLobbyUIStateClientRpc(bool gameInProgress)
        {
            lobbyUICanvas.SetActive(!gameInProgress);
        }
        
        #endregion
    }
}
