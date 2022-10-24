using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LloydDoor
{


    public class DoorEventManager : MonoBehaviour
    {
        public delegate void DoorIdle();

        public event DoorIdle DoorIdleEvent;

        void DoorIdleFunction()
        {
            DoorIdleEvent?.Invoke();
        }

        public delegate void DoorMove(int x);

        public event DoorMove DoorMoveEvent;

        void DoorMoveFunction(int x)
        {
            DoorMoveEvent?.Invoke(x);
        }

    }
}