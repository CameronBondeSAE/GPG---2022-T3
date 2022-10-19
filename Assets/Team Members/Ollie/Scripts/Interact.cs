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
        Vector3 dropPoint = transform.position + transform.forward * 5;
        if (heldItems > 0 && heldObject != null)
        {
            heldItems--;
            heldObject.GetComponent<Item>().GetDropPointClientRpc(dropPoint);
            StartCoroutine(ClientDelay());
        }
    }

    //HACK: Occasionally the object shows up for a frame in it's original position
    //before teleporting to it's correct position. WFS delay doesn't fix for some reason.
    private IEnumerator ClientDelay()
    {
        yield return new WaitForSeconds(0.5f);
        heldObject.GetComponent<Item>().GetDroppedClientRpc();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.GetComponent<IPickupable>() != null)
            {
                //objectNearby = other.gameObject;
                other.transform.parent.gameObject.SetActive(false);
                heldObject = other.gameObject;
                heldItems++;
                heldObject.GetComponent<Item>().GetPickedUpClientRpc();
            }
        }
    }
}
