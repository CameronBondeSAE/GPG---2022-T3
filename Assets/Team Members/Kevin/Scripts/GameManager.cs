using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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

        public LobbySceneManager lobbySceneManager;
        /*public delegate void OnGameStart();

        public static event OnGameStart onGameStart; 
        
        public delegate void OnGameEnd();

        public static event OnGameEnd onGameEnd;*/

        #region Component Startups

        public void Awake()
        {
            singleton = this;
            //lobbySceneManager = GetComponent<LobbySceneManager>(); 
        }

        public void OnEnable()
        {
            lobbySceneManager.OnGameStart += OnGameStarted;
            //lobbySceneManager.OnStart += OnGameStarted;
        }

        #endregion
      

        //Remove extra camera and extra lighting here
        //And the lobby canvas via SetActive
        private void OnGameStarted()
        {
            if (IsServer)
            {
                //logic to start game for host
                Debug.Log("To Host: Game has Started!!!");
                RequestOnGameStartClientRpc();
            }
        }

        [ClientRpc]
        public void RequestOnGameStartClientRpc()
        {
            if (IsClient)
            {
                //logic to start game sent to client
                Debug.Log("To Client: Game has Started!!!");
            }
        }
    }
}

