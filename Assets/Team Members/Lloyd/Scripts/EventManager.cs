using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lloyd
{
    public class EventManager : MonoBehaviour
    {
        public delegate void TerrainClear();

        public static event TerrainClear TerrainClearEvent;

        public static void TerrainClearFunction()
        {
            if (TerrainClearEvent != null)
            {
                TerrainClearEvent();
            }
        }

        public delegate void ChangeHealth(float amount);

        public static event ChangeHealth ChangeHealthEvent;

        public static void ChangeHealthFunction(float amount)
        {
            if (ChangeHealthEvent != null)
            {
                ChangeHealthEvent(amount);
            }
        }
        
        public delegate void Burning();

        public static event Burning BurningEvent;

        public static void BurningEventFunction()
        {
            if (BurningEvent != null)
            {
                BurningEvent();
            }
        }
        
        public delegate void Burnt();

        public static event Burnt BurntEvent;

        public static void BurntEventFunction()
        {
            if (BurntEvent != null)
            {
                BurntEvent();
            }
        }
        
        public delegate void DoorMove();

        public static event DoorMove DoorMoveEvent;

        public static void DoorMoveFunction()
        {
            if (DoorMoveEvent != null)
            {
                DoorMoveEvent();
            }
        }
        
        public delegate void DoorInteracted();

        public static event DoorInteracted DoorInteractedEvent;

        public static void DoorInteractedFunction()
        {
            if (DoorInteractedEvent != null)
            {
                DoorInteractedEvent();
            }
        }
        
        public delegate void DoorIdle();

        public static event DoorIdle DoorIdleEvent;

        public static void DoorIdleEventFunction()
        {
            if (DoorIdleEvent != null)
            {
                DoorIdleEvent();
            }
        }

    }
}
