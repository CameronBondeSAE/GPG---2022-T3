using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientEntity : MonoBehaviour
{
    public string playerName;

    private PlayerController _playerController;
    private GameObject controlledPlayer;

    public GameObject ControlledPlayer
    {
        get => controlledPlayer;
        set
        {
            controlledPlayer = value;
            _playerController.player = value;
            _playerController.playerTransform = value.transform;
        }
    }

    public void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();
    }
}