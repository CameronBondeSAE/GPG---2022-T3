using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kevin
{
    public class GameManager : NetworkBehaviour
    {
        //Keep track of total points/parts collected
        //Keep track of Amount of players left
        //Events for the start of game and win conditions 
        //all networked
        //Spawner maybe
        
        public int playersInLobby;
        public static GameManager singleton;
        
        //Other Components
        public LobbySceneManager lobbySceneManager;

        public LightCamManager lightCamManager;

        public AudioManager audioManager;

        public Transform localPlayerTransform;
        
        [SerializeField] private GameObject avatarPrefab;
        
        [Header("Camera Prefab")] 
        //public GameObject camera;
        
        [Header("Game State")]
        public GameState State;
        /*public delegate void OnGameStart();

        public event OnGameStart onGameStart; 
        
        public delegate void OnGameEnd();

        public event OnGameEnd onGameEnd;*/

        //Events
        public event Action<GameState> OnGameStateChanged;
        
        #region Component Startups

        public void Awake()
        {
            singleton = this;
        }

        public void OnEnable()
        {
            //lobbySceneManager.OnGameStart += OnGameStarted;
            //lobbySceneManager.OnStart += OnGameStarted;
        }
        
        #endregion

        /*private void OnGameStarted()
        {
            if (IsServer)
            {
                lobbySceneManager.lobbyCanvas.SetActive(false);
                lightCamManager.OnStarted();
                Debug.Log("To Host: Game has Started!!!");
                RequestOnGameStartClientRpc();
            }
        }*/
        
        #region Game End State

        //event/functions for the game end

        #endregion

        public void UpdateGameStates(GameState newState)
        {
            State = newState;
            switch (newState)
            {
                case GameState.IPSelect:
                    break;
                case GameState.InGameLobby:
                    HandleInGameLobby();
                    break;
                case GameState.GameStart:
                    HandleGameStart();
                    //function
                    break;
                case GameState.GameEnd:
                    HandleGameEnd();
                    //function
                    break;
            }

            OnGameStateChanged?.Invoke(newState);
        }

        private void HandleInGameLobby()
        {
            //function to run for this game state
        }

        #region Handle Game Start State

        private void HandleGameStart()
        {
            //function to run for this game state
            //disabling unneeded UI
            //enabling/spawning needed UI
            //Get the amount of players in the server
            
            if (IsServer)
            {
                lobbySceneManager.lobbyCanvas.SetActive(false);
                lightCamManager.OnStarted();
                Debug.Log("To Host: Game has Started!!!");
                RequestOnGameStartClientRpc();
            }
        }
        
        [ClientRpc]
        private void RequestOnGameStartClientRpc()
        {
            if (IsClient)
            {
                //logic to start game sent to client
                lobbySceneManager.lobbyCanvas.SetActive(false);
                lightCamManager.OnStarted();
                Debug.Log("To Client: Game has Started!!!");
                /*localPlayerTransform = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.gameObject.transform;
                Debug.Log("code reached here");
                Instantiate(camera, transform.position, Quaternion.identity, localPlayerTransform);*/
                
                /*if (IsLocalPlayer)
                {
                    localPlayerTransform = NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.gameObject.transform;
                    Debug.Log("code reached here");
                    Instantiate(camera, transform.position, Quaternion.identity, localPlayerTransform);
                    //Instantiate(camera,NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.gameObject.transform,Quaternion.identity)
                    //camera.transform.SetParent(localPlayerTransform);
                    //NetworkManager.LocalClient.PlayerObject.GetComponent<ClientEntity>().ControlledPlayer.gameObject.transform.SetParent(camera.transform);
                }*/
            }
        }

        #endregion
       

        private void HandleGameEnd()
        {
            //function to run for this game state
        }
        
        //testing game state for game manager
        
        public enum GameState
        {
            IPSelect,
            InGameLobby,
            GameStart,
            GameEnd
        }

        #region Luke's Avatar Addition

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
                client.Value.PlayerObject.GetComponent<Luke.ClientEntity>()
                    .AssignAvatarClientRpc(avatar.GetComponent<NetworkObject>().NetworkObjectId);
            }
        }
        
        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += SubscribeToSceneEvent;
        }

        #endregion
     
        
    }
}

