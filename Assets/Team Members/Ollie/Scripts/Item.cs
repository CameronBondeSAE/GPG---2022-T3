using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : NetworkBehaviour, IPickupable, IGoalItem
{
    public Renderer renderer;
    public Transform parentTransform;
    public Rigidbody rigidbody;
    private Transform tempParentTransform;
    //public BoxCollider boxCollider;
    public BoxCollider[] boxColliders;
    public bool isHeld { get; set; }

    
    void OnGUI()
    {
        //can't use built-in Start because it's called before host/client's are set
        if (GUILayout.Button("Start"))
        {
            if (IsServer)
            {
                parentTransform = transform.parent.GetComponent<Transform>();
                renderer = parentTransform.GetComponentInChildren<Renderer>();
                rigidbody = parentTransform.GetComponentInChildren<Rigidbody>();
                //boxCollider = parentTransform.GetComponentInChildren<BoxCollider>();
                boxColliders = parentTransform.GetComponentsInChildren<BoxCollider>();
                isHeld = false;
                SetupClientRpc();
            }
        }
    }

    [ClientRpc]
    void SetupClientRpc()
    {
        isHeld = false;
        parentTransform = transform.parent.GetComponent<Transform>();
        renderer = parentTransform.GetComponentInChildren<Renderer>();
        rigidbody = parentTransform.GetComponentInChildren<Rigidbody>();
    }
    
    #region IPickupable Interface

    public void PickedUp(Transform newParentTransform)
    {
        //can't send the transform via Rpc so storing it separately
        //unsure if wise for networking?
        if (!isHeld)
        {
            tempParentTransform = newParentTransform;
            isHeld = true;
        
            if (IsClient)
            {
                RequestPickUpServerRpc();
            }
        }
    }

    public void PutDown()
    {
        tempParentTransform = null;
        isHeld = false;
        
        if (IsClient)
        {
            RequestPutDownServerRpc();
        }
    }

    

    [ServerRpc]
    void RequestPickUpServerRpc()
    {
        rigidbody.isKinematic = true;
        parentTransform.transform.position = tempParentTransform.position + Vector3.forward;
        parentTransform.parent = tempParentTransform;
        //boxCollider.enabled = false;
        foreach (BoxCollider collider in boxColliders)
        {
            collider.enabled = false;
        }
        PickUpViewClientRpc();
    }

    [ServerRpc (RequireOwnership = false)]
    void RequestPutDownServerRpc()
    {
        rigidbody.isKinematic = false;
        parentTransform.parent = null;
        foreach (BoxCollider collider in boxColliders)
        {
            collider.enabled = true;
        }
        //boxCollider.enabled = true;
        PutDownViewClientRpc();
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
