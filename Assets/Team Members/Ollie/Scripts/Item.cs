using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : NetworkBehaviour, IPickupable
{
    public Renderer renderer;
    public Transform parentTransform;
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            parentTransform = transform.parent.GetComponent<Transform>();
            rigidbody = parentTransform.GetComponentInChildren<Rigidbody>();
        }
        renderer = parentTransform.GetComponentInChildren<Renderer>();
    }
    
    #region IPickupable Interface

    public void PickedUp(Transform newParentTransform)
    {
        if (IsServer)
        {
            rigidbody.isKinematic = true;
            parentTransform.transform.position = newParentTransform.position;
            parentTransform.parent = newParentTransform;
            print("picked up");
            PickUpViewClientRpc();
        }
    }

    public void PutDown()
    {
        if (IsServer)
        {
            rigidbody.isKinematic = false;
            parentTransform.parent = null;
            print("put down");
            PutDownViewClientRpc();
        }
    }

    [ClientRpc]
    private void PickUpViewClientRpc()
    {
        renderer.enabled = false;
    }

    [ClientRpc]
    private void PutDownViewClientRpc()
    {
        renderer.enabled = true;
    }

    #endregion
}
