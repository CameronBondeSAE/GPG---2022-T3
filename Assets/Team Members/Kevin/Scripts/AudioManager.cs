using System.Collections;
using System.Collections.Generic;
using Kevin;
using Luke;
using Unity.Netcode;
using UnityEngine;

public class AudioManager : NetworkBehaviour
{
    [SerializeField] private List<AudioSource> _audioSource;

    void Awake()
    {
        KGameManager.singleton.OnGameStateChanged += GameManagerOnOnGameStateChanged;
       
    }

    void OnDisable()
    {
        KGameManager.singleton.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }
    private void GameManagerOnOnGameStateChanged(KGameManager.GameState state)
    {
        if (state == KGameManager.GameState.InGameLobby)
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
