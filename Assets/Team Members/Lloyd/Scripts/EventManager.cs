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
        
        public delegate void DoorOpen();

        public static event DoorOpen DoorOpenEvent;

        public static void DoorOpenEventFunction()
        {
            if (DoorOpenEvent != null)
            {
                DoorOpenEvent();
            }
        }
        
        public delegate void DoorClose();

        public static event DoorClose DoorCloseEvent;

        public static void DoorCloseEventFunction()
        {
            if (DoorCloseEvent != null)
            {
                DoorCloseEvent();
            }
        }

    }
}
