using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Luke;
using Unity.Netcode;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
    public delegate void ItemPlacedEventAction(int amount);
    public event ItemPlacedEventAction itemPlacedEvent;
    public NetworkVariable<Color> colorRed;

    private Renderer renderer;
    private HQ.HQType hqType;

    private Renderer rend;

    public int amount;
    public int goalAmount;
    private void Start()
    {
        colorRed = new NetworkVariable<Color>(Color.red);
        rend = GetComponent<Renderer>();
        rend.material.color = colorRed.Value;
        if (IsServer)
        {
            hqType = GetComponentInParent<HQ>().type;
        }

        goalAmount = GameManager.singleton.targetEndResources;

        //HACK: just to get the UI score denominator
        itemPlacedEvent?.Invoke(amount);
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
                    CheckpointUpdateClientRpc();
                    if (amount >= goalAmount)
                    {
                        GameManager.singleton.InvokeOnGameEnd();
                    }
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
