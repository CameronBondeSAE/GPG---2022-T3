using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;

public class DoorIdleState : MonoBehaviour

{
    
    private void OnEnable()
    {
        EventManager.DoorInteractedEvent += ChangeState;
    }

    private void ChangeState()
    {
        EventManager.DoorMoveFunction();
        Debug.Log("hi");
    }

    private void OnDisable()
    {
        EventManager.DoorInteractedEvent -= ChangeState;
    }
}
