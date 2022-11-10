using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;

public class DoorMovingState : MonoBehaviour
{
    private DoorDoubleModel _doorModel;

    private Rigidbody _rb01;
    private Rigidbody _rb02;

    private GameObject _doorWing01;
    private GameObject _doorWing02;

    private bool _isMoving;

    private float _speed;
    private int _timeMoving;
    private bool _isOpen;

    private int _fixedUpdateCount;

    public DoorEventManager _doorEvent;

    private void OnEnable()
    {
        _doorModel = GetComponent<DoorDoubleModel>();

        _doorWing01 = _doorModel.Wing01();
        _rb01 = _doorWing01.GetComponent<Rigidbody>();

        _doorWing02 = _doorModel.Wing02();
        _rb02 = _doorWing02.GetComponent<Rigidbody>();

        _speed = _doorModel.GetSpeed();
        _timeMoving = _doorModel.GetTimeMoving();

        _isOpen = _doorModel.IsOpen();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        _fixedUpdateCount = 0;
        _isMoving = true;
        
        if (_isOpen)
        {
            _rb01.AddForce(_rb01.transform.position + (Vector3.left * _speed));
            _rb02.AddForce(_rb02.transform.position + (Vector3.right * _speed));
        }
        else
        {
            _rb01.AddForce(_rb01.transform.position + (Vector3.right * _speed));
            _rb02.AddForce(_rb02.transform.position + (Vector3.left * _speed));
        }

        yield return new WaitUntil(() => _fixedUpdateCount >= _timeMoving);
        
        _rb01.velocity = new Vector3(0f, 0f, 0f);
        _rb02.velocity = new Vector3(0f, 0f, 0f);

        _isMoving = false;

        _doorEvent.DoorIdleFunction();
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            _fixedUpdateCount++;
        }
    }

    private void OnDisable()
    {
        _rb01.velocity = new Vector3(0f, 0f, 0f);
        _rb02.velocity = new Vector3(0f, 0f, 0f);
    }
}