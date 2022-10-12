using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
    //called when item placed, maybe player listens to show score?
    public event Action itemPlacedEvent;
    
    private void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;
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
        GetComponent<Renderer>().material.color = Color.green;
    }
}
