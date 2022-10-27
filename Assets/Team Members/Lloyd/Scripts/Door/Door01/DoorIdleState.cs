using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;

public class DoorIdleState : MonoBehaviour

{
    private void OnEnable()
    {   
        EventManager.singleton.DoorInteractedEvent += ChangeState;
    }

    private void ChangeState()
    {
        EventManager.singleton.DoorMoveFunction();
    }

    private void OnDisable()
    {
        EventManager.singleton.DoorInteractedEvent -= ChangeState;
    }
}
