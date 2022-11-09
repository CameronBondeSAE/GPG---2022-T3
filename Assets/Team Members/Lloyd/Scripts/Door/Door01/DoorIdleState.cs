using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;

public class DoorIdleState : MonoBehaviour

{
    public DoorEventManager _doorEvent;
    
    private void OnEnable()
    {   
        _doorEvent.DoorInteractedEvent += ChangeState;
    }

    private void ChangeState()
    {
        _doorEvent.DoorMoveFunction();
    }

    private void OnDisable()
    {
        _doorEvent.DoorInteractedEvent -= ChangeState;
    }
}
