using System.Collections;
using System.Collections.Generic;
using Luke;
using Unity.Netcode;
using UnityEngine;
using GameManager = Kevin.GameManager;

public class AudioManager : NetworkBehaviour
{
    [SerializeField] private List<AudioSource> _audioSource;

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
       
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }
    private void GameManagerOnOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.InGameLobby)
        {
            _audioSource[0].Play();
            RequestClientAudioPlayClientRpc();
        }
    }

    [ClientRpc]
    private void RequestClientAudioPlayClientRpc()
    {
        //_audioSource[0].Play();
    }
}
