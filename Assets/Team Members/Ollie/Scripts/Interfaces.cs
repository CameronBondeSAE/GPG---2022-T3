using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPickupable
{
    void PickedUp();
    void PutDown();
    bool isHeld { get; set; }
    bool locked { get; set; }
}

public interface IGoalItem
{
    
}

public interface ILateSync
{
    
}
