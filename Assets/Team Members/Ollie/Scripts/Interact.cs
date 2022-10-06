using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : NetworkBehaviour
{
    public GameObject objectNearby;
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
        if (heldObject != null && heldObject.GetComponent<IPickupable>().isHeld)
        {
            heldObject.GetComponent<IPickupable>().PutDown();
            heldObject = null;
        }
        else if (objectToInteract == null) return;
        else if (!objectToInteract.GetComponent<IPickupable>().isHeld)
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
    
    //currently Exit is not being called when Item's colliders are disabled
    //so if two players are touching the same Item and one picks up and drops far away
    //the other can teleport the item to them, no matter distance
    //bueno = null :(
    
    //TODO: 
    //rework this to be destroy item and re-instantiate it on PutDown
    private void OnTriggerExit(Collider other)
    {
        if (IsServer)
        {
            objectNearby = null;
        }
    }
}
