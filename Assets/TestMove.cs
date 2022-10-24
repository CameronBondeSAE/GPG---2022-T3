using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Tools.NetStatsMonitor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TestMove : MonoBehaviour
{
    private Rigidbody _rb;

    private DoorComponents _doorComp;

    [SerializeField] private float _speed;

    [SerializeField] private bool isMoving = false;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _doorComp = GetComponent<DoorComponents>();

        Lloyd.EventManager.DoorInteractedEvent += Move;
        _rb.AddForce(_rb.transform.position + (Vector3.left * _speed));

    }

    private void Move()
    {
       // isMoving = !isMoving;
       Debug.Log("HI");
    }

    private void FixedUpdate()
    {
       // if(isMoving)
        //_rb.MovePosition(_rb.transform.position + (Vector3.left * _speed));
        
    }


    private void OnDisable()
    {
        Lloyd.EventManager.DoorInteractedEvent -= Move;
    }
}
