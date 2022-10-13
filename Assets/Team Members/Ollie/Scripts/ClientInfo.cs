using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using System;
using Ollie;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ClientInfo : NetworkBehaviour
{
    public string clientName = "Player";
    public NetworkVariable<FixedString512Bytes> ClientName = new NetworkVariable<FixedString512Bytes>();
    public GameObject lobbyUIRef;

    public event Action<string> onNameChangeEvent;

    public override void OnNetworkSpawn()
    {
        ClientName.OnValueChanged += OnNameChange;
    }

    private void OnNameChange(FixedString512Bytes previousValue, FixedString512Bytes newValue)
    {
        onNameChangeEvent?.Invoke(newValue.ToString());

        if (lobbyUIRef != null)
        {
            lobbyUIRef.GetComponent<TMP_Text>().text = newValue.ToString();
        }
    }

    public void Init(ulong clientId)
    {
        ClientName.Value = ("Player " + clientId);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (lobbyUIRef != null)
        {
            Destroy(lobbyUIRef);
        }

        if (LobbyUIManager.instance != null)
        {
            LobbyUIManager.instance.RequestClientUIUpdateServerRpc();
        }
    }
}
