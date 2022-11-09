using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;

public class DoorEventManager : MonoBehaviour
{
    public delegate void ChangeHealth(float amount);

    public event ChangeHealth ChangeHealthEvent;

    public void ChangeHealthFunction(float amount)
    {
        if (ChangeHealthEvent != null)
        {
            ChangeHealthEvent(amount);
        }
    }
    
            public delegate void Burning();
    
            public event Burning BurningEvent;
    
            public void BurningEventFunction()
            {
                if (BurningEvent != null)
                {
                    BurningEvent();
                }
            }
    
            public delegate void Burnt();
    
            public event Burnt BurntEvent;
    
            public void BurntEventFunction()
            {
                if (BurntEvent != null)
                {
                    BurntEvent();
                }
            }
    
            public delegate void DoorMove();
    
            public event DoorMove DoorMoveEvent;
    
            public void DoorMoveFunction()
            {
                if (DoorMoveEvent != null)
                {
                    DoorMoveEvent();
                }
            }
    
            public delegate void DoorInteracted();
    
            public event DoorInteracted DoorInteractedEvent;
    
            public void DoorInteractedFunction()
            {
                if (DoorInteractedEvent != null)
                {
                    DoorInteractedEvent();
                }
            }
    
            public delegate void DoorIdle();
    
            public event DoorIdle DoorIdleEvent;
    
            public void DoorIdleFunction()
            {
                DoorIdleEvent?.Invoke();
            }
}
