using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
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
