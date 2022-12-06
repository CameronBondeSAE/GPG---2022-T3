using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Luke
{
    public class ClientEntity : NetworkBehaviour
    {
        public string playerName;

        private PlayerController _playerController;
        [SerializeField] private GameObject controlledPlayer;

        public GameObject ControlledPlayer
        {
            get => controlledPlayer;
            set
            {
                controlledPlayer = value;
                _playerController.player = value;
                _playerController.playerTransform = value.transform;
                // _playerController.playerControls.Player.Enable();
            }
        }

        public void OnEnable()
        {
            _playerController = GetComponent<PlayerController>();
        }
    }
}