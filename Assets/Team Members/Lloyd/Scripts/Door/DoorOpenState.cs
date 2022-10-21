using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenState : MonoBehaviour
{
    private void OnEnable()
    {
        Lloyd.EventManager.DoorOpenEventFunction();
    }
}
