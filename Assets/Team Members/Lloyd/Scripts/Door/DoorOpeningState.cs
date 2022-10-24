using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;

public class DoorOpeningState : MonoBehaviour
{
    private DoorComponents _comp;

    private float _speed;

    private void Move()
    {
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        _comp = GetComponent<DoorComponents>();
                _speed = _comp.GetSpeed();
                Lloyd.EventManager.DoorOpenEventFunction();
    }

    private void OnDisable()
    {
    }
}