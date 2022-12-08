using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Unity.Netcode;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
    //called when item placed, maybe player listens to show score?
    public delegate void ItemPlacedEventAction(int amount);
    public event ItemPlacedEventAction itemPlacedEvent;
    public NetworkVariable<Color> colorRed;

    private Renderer renderer;
    private HQ.HQType hqType;

    private Renderer rend;

    public int amount;
    private void Start()
    {
        colorRed = new NetworkVariable<Color>(Color.red);
        rend = GetComponent<Renderer>();
        rend.material.color = colorRed.Value;
        // GetComponent<NetworkObject>().Spawn();
        if (IsServer)
        {
            hqType = GetComponentInParent<HQ>().type;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer && hqType == HQ.HQType.Humans)
        {
            Interact player = other.GetComponentInParent<Interact>();
            if (player != null)
            {
                if (player.storedItems > 0)
                {
                    amount += player.storedItems;
                    player.ResetHeadScore();
                    itemPlacedEvent?.Invoke(amount);
                    print("item Placed Event invoked for item count = " +amount);
                    CheckpointUpdateClientRpc();
                }
            }
        }
    }

    [ClientRpc]
    void CheckpointUpdateClientRpc()
    {
        StartCoroutine(CheckpointReceiveItems());
    }

    public IEnumerator CheckpointReceiveItems()
    {
        //TODO make it something funky
        rend.material.color = Color.green;
        yield return new WaitForSeconds(2f);
        rend.material.color = Color.red;
    }
}
