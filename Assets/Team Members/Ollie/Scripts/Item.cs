using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour, IPickupable
{
    public Renderer renderer;
    public Transform parentTransform;
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent.GetComponent<Transform>();
        renderer = parentTransform.GetComponentInChildren<Renderer>();
        rigidbody = parentTransform.GetComponentInChildren<Rigidbody>();
    }
    
    #region IPickupable Interface

    public void PickedUp(Transform newParentTransform)
    {
        rigidbody.isKinematic = true;
        renderer.enabled = false;
        parentTransform.transform.position = newParentTransform.position;
        parentTransform.parent = newParentTransform;
        print("picked up");
    }

    public void PutDown()
    {
        renderer.enabled = true;
        rigidbody.isKinematic = false;
        parentTransform.parent = null;
        print("put down");
    }

    #endregion
}
