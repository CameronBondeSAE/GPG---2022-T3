using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPickupable
{
    void PickedUp(GameObject interactor);
    void PutDown(GameObject interactor);
    bool isHeld { get; set; }
    bool locked { get; set; }
}

public interface IGoalItem
{
    
}

public interface ILateSync
{
    
}
