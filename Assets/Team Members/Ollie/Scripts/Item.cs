using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour, IPickupable
{
    public Renderer renderer;
    public EmptyParent emptyParent;

    // Start is called before the first frame update
    void Start()
    {
        emptyParent = GetComponentInParent<EmptyParent>();
        renderer = emptyParent.GetComponentInChildren<Renderer>();
    }
    
    #region IPickupable Interface

    public void PickedUp()
    {
        renderer.enabled = false;
        print("picked up");
    }

    public void PutDown()
    {
        renderer.enabled = true;
        print("put down");
    }

    #endregion
}
