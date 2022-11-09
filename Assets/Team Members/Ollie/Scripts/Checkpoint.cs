using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
    //called when item placed, maybe player listens to show score?
    public event Action itemPlacedEvent;
    public NetworkVariable<Color> colorRed;

    private void Start()
    {
        colorRed = new NetworkVariable<Color>(Color.red);
        GetComponent<Renderer>().material.color = colorRed.Value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            Transform otherParent = other.transform.parent;
            if (otherParent != null && other.transform.parent.GetComponentInChildren<IGoalItem>() != null)
            {
                GetComponent<Renderer>().material.color = Color.green;
                otherParent.GetComponentInChildren<Item>().locked = true;
                itemPlacedEvent?.Invoke();
                CheckpointUpdateClientRpc();
            }
        }
    }

    [ClientRpc]
    void CheckpointUpdateClientRpc()
    {
        //TODO change this to ITEM colour, not checkpoint
        //unless we're having one item per checkpoint
        GetComponent<Renderer>().material.color = Color.green;
    }
}
