using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Luke
{
    public class ClientEntity : NetworkBehaviour
    {
        public string playerName;

        private ClientInfo _clientInfo;
        
        
        private PlayerController _playerController;
        [SerializeField] private GameObject controlledPlayer;

        public GameObject ControlledPlayer
        {
            get => controlledPlayer;
            set
            {
                controlledPlayer = value;
                if (!IsServer) return;
                _playerController.player = value;
                _playerController.playerTransform = value.transform;
                // _playerController.playerControls.Player.Enable();
            }
        }

        public void OnEnable()
        {
            _playerController = GetComponent<PlayerController>();
            _clientInfo = GetComponent<ClientInfo>();
        }

        
    }
}