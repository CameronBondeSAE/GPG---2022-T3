using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : NetworkBehaviour
{
    private GameObject objectNearby;
    private GameObject heldObject;

    private void Update()
    {
        if (IsLocalPlayer)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                RequestInteractWithServerRpc();
            }
        }
    }

    [ServerRpc]
    private void RequestInteractWithServerRpc()
    {
        InteractWith(objectNearby);
    }
    
    private void InteractWith(GameObject objectToInteract)
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<IPickupable>().PutDown();
            heldObject = null;
        }
        else if (objectToInteract == null) return;
        else
        {
            objectToInteract.GetComponent<IPickupable>().PickedUp(transform);
            heldObject = objectToInteract;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.GetComponent<IPickupable>() != null)
            {
                objectNearby = other.gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsServer)
        {
            objectNearby = null;
        }
    }
}
