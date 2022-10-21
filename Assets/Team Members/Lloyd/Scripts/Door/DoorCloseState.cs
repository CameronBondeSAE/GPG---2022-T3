using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseState : MonoBehaviour
{
        private void OnEnable()
        {
            Lloyd.EventManager.DoorCloseEventFunction();
        }
}