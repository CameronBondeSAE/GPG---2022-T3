using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
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

        public GameObject levelsListPanel;
        public GameObject levelButtonPrefab;

        public GameObject lobbyUICanvas;

        public TMP_Text clientUI;
        public Button startButton;
        public Button lobbyButton;
        public TMP_InputField playerNameInputField;
        public GameObject levelsUI;
        public GameObject waitForHostBanner;
        public GameObject playerListPanel;
        public GameObject clientLobbyUIPrefab;

        [Header("IP Canvas Setup")]
        public GameObject ipAddressCanvas;

        public TMP_InputField serverIPInputField;
        
        [Header("Hack for now/Ignore")]
        public GameObject playerPrefab;

        public GameObject lobbyCam;
        bool              inGame = false;

        ulong         myLocalClientId;
        NetworkObject myLocalClient;
        string        clientName;

        public static LobbyUIManager instance;

        public void HostGame()
        {
            NetworkManager.Singleton.StartHost();

            if (!autoHost)
            {
                //activate lobby canvas
                //DEactivate ip canvas

                foreach (Level level in levels)
                {
                    GameObject levelButton = Instantiate(levelButtonPrefab, levelsListPanel.transform);
                    levelButton.GetComponentInChildren<TMP_Text>().text = level.levelNameOnUI;
                    levelButton.GetComponent<LevelButton>().myLevel = level.level.name;
                }
            }
        }
        
        private void Awake()
        {
            if (!autoHost)
            {
                //set up IP address canvas here
                //disable lobby canvas
            }

            instance = this;
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;

            serverIPInputField.text = NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress;

            if (autoHost)
            {
                //Host Game
            }

            if (autoLoadLevel)
            {
                //Start Game
            }
        }

        public void OnNewServerIPAddress()
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = serverIPInputField.text;
        }

        public void StartGame()
        {
            if (sceneToLoad == "")
            {
                print("You must select a level to load.");
                return;
            }

            NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManagerOnOnSceneEvent;
        }

        private void SceneManagerOnOnSceneEvent(SceneEvent sceneEvent)
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManagerOnOnSceneEvent;
            Scene scene = sceneEvent.Scene;
            
            //update ui
            //disable lobby camera
            //clientrpc an updated ui
        }
        
        private void OnClientJoin(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer || IsOwner)
            {
                NetworkClient client;
                if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out client))
                {
                    ClientInfo clientInfo = client.PlayerObject.GetComponent<ClientInfo>();
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestClientUIUpdateServerRpc()
        {
            HandleClientNameChange();
        }

        void HandleClientNameChange()
        {
            // Clear Lobby Names Client Rpc

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                SpawnClientLobbyUIClientRpc(client.PlayerObject.GetComponent<ClientInfo>().ClientName.Value.ToString());
            }
        }

        [ClientRpc]
        public void SpawnClientLobbyUIClientRpc(string newName)
        {
            SpawnClientLobbyUIClientRpc(newName);
        }

        void SpawnClientLobbyUI(string clientName)
        {
            // GameObject uiRef = Instantiate((clientLobbyUIPrefab, clientField.transform));
            // uiRef.GetComponent<TMP_Text>().text = clientName;
            // NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<ClientInfo>().lobbyUIRef = uiRef;
        }
    }
}
