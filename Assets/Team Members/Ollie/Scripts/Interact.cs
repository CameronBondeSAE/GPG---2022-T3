using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Put this on the Player
/// allows them to pickup plants & items
/// also to open doors and use items
/// </summary>
public class Interact : NetworkBehaviour
{
    public GameObject heldObject;
    public Transform equippedMountPos;
    public IPickupable pickupableNearby;
    public int storedItems;
    public int storedMax;
    public int equippedItems;
    public int equippedMax;

    [ServerRpc]
    public void RequestInteractWithServerRpc()
    {
        if (pickupableNearby != null)
        {
            if (equippedItems < equippedMax)
            {
                equippedItems++;
                pickupableNearby.PickedUp(gameObject);
                //somehow set the object to be my child
                //and place it in equippedMountPos;
            }
        }
        
        
        
        Vector3 dropPoint = transform.position + transform.forward * 2;
        if (storedItems > 0 && heldObject != null)
        {
            storedItems--;
            heldObject.GetComponent<ItemBase>().GetDropPointClientRpc(dropPoint);
            heldObject.GetComponent<ItemBase>().GetDroppedClientRpc();
        }
    }

    public void ParentItemObject(GameObject child)
    {
        child.transform.parent = equippedMountPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPickupable>() != null)
        {
            IPickupable item = other.GetComponent<IPickupable>();
            if (item.autoPickup)
            {
                if (storedItems < storedMax)
                {
                    item.PickedUp(gameObject);
                    item.DestroySelf();
                    storedItems++;
                }
                else
                {
                    print("You've got too many plants!");
                }
            }
            else //ie, not a plant
            {
                pickupableNearby = item;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPickupable>() != null)
        {
            pickupableNearby = null;
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (IsServer)
        {
            if (other.GetComponent<IPickupable>() != null)
            {
                if (storedItems == 0)
                {
                    if (!other.gameObject.GetComponent<ItemBase>().locked)
                    {
                        other.transform.parent.gameObject.SetActive(false);
                        heldObject = other.gameObject;
                        storedItems++;
                        heldObject.GetComponent<ItemBase>().GetPickedUpClientRpc();
                    }
                }
                else print("You're already holding an item! Press space to drop it.");
            }
            
        }
    }*/
}
