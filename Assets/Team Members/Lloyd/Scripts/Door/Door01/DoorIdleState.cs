using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;

public class DoorIdleState : MonoBehaviour

{ 
    
    
    private void OnEnable()
    {
        EventManager.DoorIdleFunction();
        
        EventManager.DoorInteractedEvent += ChangeState;
    }

    private void ChangeState()
    {
        Debug.Log("hi");
        EventManager.DoorMoveFunction();
    }

    private void OnDisable()
    {
        EventManager.DoorInteractedEvent -= ChangeState;
    }
}
