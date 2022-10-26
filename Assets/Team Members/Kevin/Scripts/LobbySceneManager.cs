using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using TMPro;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Unity.Netcode.Transports.UTP;

namespace Kevin
{
    [Serializable]
    public class Level
    {
        public Object level;
        public string levelNameOnUI;
    }
    
    public class LobbySceneManager : NetworkBehaviour
    {

        [Header("Level Manager")] public List<Level> levels;
        
        //Level Selecter Manager
        public static LobbySceneManager instance;
        public string selectedLevel;
        public string selectedSceneToLoad;
        
        public GameObject levelPrefab;

        public Transform selectLevelPanelTransform;
        public Transform playerPanelTransform;

        private Transform updateTransform;
        //UI Manager
        
        public GameObject ipCanvas;
        public GameObject lobbyCanvas;
        public GameObject selectLevelGameObject;
        public GameObject lobbyNamePrefab;
        public Button startButton;

        public GameObject lobbyCamera;
        public GameObject lobbyDirectionalLight;
        
        public TMP_Text selectedLevelText;

        public TMP_InputField playerInputField;
        //IP Address
        public TMP_InputField ipInputField;


        ulong         myLocalClientId;
        NetworkObject myLocalClient;
        string        clientName;
        
        public event Action OnGameStart;
        
        /*public delegate void OnStartGame();
        public event OnStartGame OnStart;*/
        public void Awake()
        {
            instance = this;
        }
        
        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;

            ipInputField.text = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;
        }
        
        #region ButtonManager

        public void HostGameButton()
        {
            /*check if input is entered or not, change to specifics later
            if (ipInputField.text == "")
            {
                Debug.Log("You must type in IP Address!");
                return;
            }*/
            if (ipInputField.text == "")
            {
                Debug.Log("IP address not entered!!!");
            }
            else
            {
                GameManager.singleton.UpdateGameStates(GameManager.GameState.InGameLobby);
                NetworkManager.Singleton.StartHost();
                ipCanvas.SetActive(false);
                lobbyCanvas.SetActive(true);

                foreach (Level level in levels)
                {
                    GameObject levelObject = Instantiate(levelPrefab, selectLevelPanelTransform);
                    levelObject.GetComponentInChildren<TMP_Text>().text = level.levelNameOnUI;
                    levelObject.GetComponent<LevelButton>().currentLevel = level.level.name;
                }

                Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
            }
        }

        public void JoinGameButton()
        {
            if (ipInputField.text == "")
            {
                Debug.Log("IP address not entered!!!");
            }
            else
            {
                GameManager.singleton.UpdateGameStates(GameManager.GameState.InGameLobby);
                NetworkManager.Singleton.StartClient();
                ClientLobby();
                Debug.Log("Player just joined!");
                Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
            }
        }

        void ClientLobby()
        {
            ipCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            startButton.gameObject.SetActive(false);
            selectLevelGameObject.SetActive(false);
        }

        public void StartGameButton()
        {
            //Debug.Log("Start button pressed!!!");
            //GameManager.singleton.InvokeOnGameStart();
            //GameManager.singleton.OnGameStarted();
            OnGameStart?.Invoke();
            GameManager.singleton.UpdateGameStates(GameManager.GameState.GameStart);
            //OnStart?.Invoke();
            if (selectedLevel == "")
            {
                Debug.Log("You must select a level!!!");
                return;
            }

            try
            {
                NetworkManager.Singleton.SceneManager.LoadScene(selectedLevelText.text, LoadSceneMode.Additive);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        #endregion

        //When an IP address is entered into the text field.
        public void OnIpEntered()
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ipInputField.text;
        } 
        
        //updating the level selected.
        public void UpdateLevelText(string level)
        {
            selectedLevelText.text = level;
        }

        #region Lobby Names

        private void OnClientJoin(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer || IsOwner)
            {
                NetworkClient client;
                if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out client))
                {
                    ClientInfo clientInfo = client.PlayerObject.GetComponent<ClientInfo>();
                    clientInfo.Init((ulong) NetworkManager.Singleton.ConnectedClients.Count);

                    GameObject uiRef = Instantiate(lobbyNamePrefab, playerPanelTransform);
                    clientInfo.lobbyUIRef = uiRef;
                    uiRef.GetComponent<TMP_Text>().text = clientInfo.ClientName.Value.ToString();
                }
                HandleLocalClient(clientId);
            }
            //else RequestClientNamesLobbyUIServerRpc(clientId);

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

        void HandleClientNameChange()
        {
            ClearLobbyNamesClientRpc();

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                SpawnClientLobbyUIClientRpc(client.PlayerObject.GetComponent<ClientInfo>().ClientName.Value.ToString());
            }
        }

        [ClientRpc]
        public void ClearLobbyNamesClientRpc()
        {
            foreach (Transform child in playerPanelTransform.transform)
            {
                Destroy(child.gameObject);
            }
        }

        [ClientRpc]
        public void SpawnClientLobbyUIClientRpc(string newName)
        {
            SpawnClientLobbyUI(newName);
        }

        void SpawnClientLobbyUI(string clientName)
        {
            GameObject uiRef = Instantiate(lobbyNamePrefab, playerPanelTransform);
            uiRef.GetComponent<TMP_Text>().text = clientName;
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<ClientInfo>().lobbyUIRef = uiRef;
        }

        public void UpdateClientName()
        {
            if (IsServer)
            {
                if (myLocalClient != null)
                {
                    myLocalClient.GetComponent<ClientInfo>().ClientName.Value = playerInputField.text;
                    HandleClientNameChange();
                }
                else
                {
                    print("No local client found");
                }
            }
            else
            {
                RequestClientNameChangeServerRpc(myLocalClientId, playerInputField.text);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void RequestClientNameChangeServerRpc(ulong clientId, string name)
        {
            NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<ClientInfo>().ClientName
                .Value = name;
            HandleClientNameChange();
        }

        #endregion

        #region LeaveLobby

        

        #endregion
    }

}
