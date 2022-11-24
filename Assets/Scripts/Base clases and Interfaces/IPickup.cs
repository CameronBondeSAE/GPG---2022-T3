using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    void PickedUp(GameObject interactor);
    void PutDown(GameObject interactor);
   
    bool isHeld { get; set; }
    bool locked { get; set; }
    bool autoPickup { get; set; } 
    
    void DestroySelf();
}
