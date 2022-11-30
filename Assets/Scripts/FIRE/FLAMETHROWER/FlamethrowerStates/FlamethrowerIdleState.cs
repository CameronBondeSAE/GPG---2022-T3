using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

public class FlamethrowerIdleState: MonoBehaviour
{
    public FlamethrowerModelView modelView;

    private void OnEnable()
    {
        modelView = GetComponentInChildren<FlamethrowerModelView>();
    }

    private void OnDisable()
    {
        
    }
    
}
