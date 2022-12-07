using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPickupable
{
    void PickedUp(GameObject interactor, ulong networkObjectId);
    void PutDown(GameObject interactor, ulong networkObjectId);
    void DestroySelf();
    bool isHeld { get; set; }
    bool locked { get; set; }
    bool autoPickup { get; set; }
}

public interface IThrowOut
{
    void ThrowOut(GameObject thrower);
}

public interface IGoalItem
{
    
}

public interface ILateSync
{
    
}
