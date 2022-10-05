using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private GameObject objectNearby;
    private GameObject heldObject;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            InteractWith(objectNearby);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPickupable>() != null)
        {
            objectNearby = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectNearby = null;
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

}
