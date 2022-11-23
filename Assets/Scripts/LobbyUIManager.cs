using System;
using System.Collections;
using System.Collections.Generic;
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
        public Button lobbyButton;
        public TMP_InputField playerNameInputField;
        public TMP_Text levelSelectedDisplayText;
        public GameObject levelDisplayUI;
        public GameObject waitForHostBanner;
        public GameObject playerPanel;
        public GameObject clientLobbyUIPrefab;

        [Header("IP Canvas Setup")]
        public GameObject ipAddressCanvas;

        public TMP_InputField serverIPInputField;
        
        [Header("Hack for now/Ignore")]
        //public GameObject playerPrefab;

        //public GameObject lobbyCam;
        //public GameObject directionalLight;
        bool              inGame = false;

        ulong         myLocalClientId;
        NetworkObject myLocalClient;
        string        clientName;

        public static LobbyUIManager instance;

        
        #region Lobby Specific Stuff

        private void Awake()
        {
            if (!autoHost)
            {
                ipAddressCanvas.SetActive(true);
                lobbyUICanvas.SetActive(false);
            }

            instance = this;
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
            

            if (!autoHost)
            {
                lobbyUICanvas.SetActive(true);
                ipAddressCanvas.SetActive(false);

                foreach (Level level in levels)
                {
                    GameObject levelButton = Instantiate(levelButtonPrefab, levelHolder.transform);
                    levelButton.GetComponentInChildren<TMP_Text>().text = level.levelNameOnUI;
                    levelButton.GetComponent<LevelButton>().myLevel = level.level.name;
                }
            }
            else
            {
                //lobbyUICanvas.SetActive(false);
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
            lobbyUICanvas.SetActive(true);
            ipAddressCanvas.SetActive(false);
            levelDisplayUI.SetActive(false);
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
        
        public void UpdateLevelSelectedText(string levelName)
        {
            levelSelectedDisplayText.text = levelName;
        }
        
        #endregion

        //TODO: Rip this region into a Game Mode script, IF TIME PERMITS
        #region Game Mode Manager Stuff

        public void StartGame()
        {
            if (sceneToLoad == "")
            {
                print("You must select a level to load.");
                return;
            }

            NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManagerOnOnSceneEvent;
            NetworkManager.Singleton.SceneManager.OnLoadComplete += SetNewActiveScene;

            //use this to know when scene IS loaded
            //NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLevelLoaded;

            //if this fails it will duplicate spawns of the player?
            try
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
                //set new scene as default somehow??
            }
            catch (Exception e)
            {
                Debug.LogException(e,this);
            }
        }
        
        private void SetNewActiveScene(ulong clientid, string scenename, LoadSceneMode loadscenemode)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= SetNewActiveScene;
            Scene scene = (SceneManager.GetSceneByName(scenename));
            SceneManager.SetActiveScene(scene);
            //BroadcastActiveSceneClientRpc(scenename);
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
            if (sceneEvent.SceneEventType != SceneEventType.Load) return;
            NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManagerOnOnSceneEvent;
            Scene scene = sceneEvent.Scene;
            
            BroadcastLobbyUIStateClientRpc(true);
        }

        [ClientRpc]
        private void BroadcastLobbyUIStateClientRpc(bool gameInProgress)
        {
            lobbyUICanvas.SetActive(!gameInProgress);
        }
        
        #endregion
    }
}
