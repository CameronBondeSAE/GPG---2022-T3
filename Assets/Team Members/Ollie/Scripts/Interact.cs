using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : NetworkBehaviour
{
    public GameObject heldObject;
    public int heldItems;

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
        Vector3 dropPoint = transform.position + transform.forward * 2;
        if (heldItems > 0 && heldObject != null)
        {
            heldItems--;
            heldObject.GetComponent<Item>().GetDropPointClientRpc(dropPoint);
            heldObject.GetComponent<Item>().GetDroppedClientRpc();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsServer)
        {
            if (other.GetComponent<IPickupable>() != null)
            {
                if (heldItems == 0)
                {
                    if (!other.gameObject.GetComponent<Item>().locked)
                    {
                        other.transform.parent.gameObject.SetActive(false);
                        heldObject = other.gameObject;
                        heldItems++;
                        heldObject.GetComponent<Item>().GetPickedUpClientRpc();
                    }
                }
                else print("You're already holding at item! Press space to drop it.");
            }
            
        }
    }
}
