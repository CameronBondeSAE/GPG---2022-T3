using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using Lloyd;
using Luke;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
    public delegate void ItemPlacedEventAction(int amount);
    public event ItemPlacedEventAction itemPlacedEvent;
    public NetworkVariable<Color> colorRed;
    
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
            Avatar playerAvatar = other.GetComponentInParent<Avatar>();
            if (playerAvatar == null) return;
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
        
        

        if (IsServer && hqType == HQ.HQType.Aliens)
        {
            //HACK: Only AI have Wander script, so prevents Humans dropping in alien base
            if (other.GetComponentInParent<Wander>() != null)
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
                            //TODO: Uncomment this when figured out Alien's wincon
                            //GameManager.singleton.InvokeOnGameEnd();
                        }
                    }
                }
            }
        }
    }

    public void PlayerDied()
    {
	    amount *= 3/4;
	    itemPlacedEvent?.Invoke(amount);
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
