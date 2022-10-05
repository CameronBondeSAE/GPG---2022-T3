using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private GameObject viableObject;
    private GameObject heldObject;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponentInParent<CapsuleCollider>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            InteractWith(viableObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPickupable>() != null)
        {
            viableObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        viableObject = null;
    }

    private void InteractWith(GameObject gameObject)
    {
        if (gameObject == null && heldObject != null)
        {
            heldObject.GetComponent<IPickupable>().PutDown();
        }
        else if (gameObject == null && heldObject == null) return;
        else
        {
            gameObject.GetComponent<IPickupable>().PickedUp();
            heldObject = gameObject;
        }
    }

}
