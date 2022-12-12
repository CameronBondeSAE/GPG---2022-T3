using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HatRandomiser : NetworkBehaviour
{
    public List<GameObject> hats;

    private void Start()
    {
        if (!IsServer) return;
        var x = UnityEngine.Random.Range(0, hats.Count);
        if(hats.Count>=x) ActivateHatClientRpc(x);
    }

    [ClientRpc]
    void ActivateHatClientRpc(int i)
    {
        hats[i].SetActive(true);
    }
}
