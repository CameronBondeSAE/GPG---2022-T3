using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;

public class DoorSingleMoveState : MonoBehaviour
{
    private DoorModel _doorModel;

    private Rigidbody rb;

    private Vector3 doorPos;

    [SerializeField] private float maxY;

    private bool _isMoving;

    private float _speed;
    private int _timeMoving;
    
    private bool _isOpen;

    private int _fixedUpdateCount;

    public DoorEventManager _doorEvent;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        
        doorPos = transform.position;
        
        _doorModel = GetComponent<DoorModel>();

        _isOpen = _doorModel.IsOpen();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        _fixedUpdateCount = 0;
        _isMoving = true;
        
        if (_isOpen)
        {
            transform.position = Vector3.MoveTowards(doorPos, new Vector3(doorPos.x, doorPos.y-maxY, doorPos.z), _speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(doorPos, new Vector3(doorPos.x, doorPos.y+maxY, doorPos.z), _speed);
        }

        yield return new WaitUntil(() => _fixedUpdateCount >= _timeMoving);

        doorPos = transform.position;
        
        rb.velocity = new Vector3(0f, 0f, 0f);

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
        rb.velocity = new Vector3(0f, 0f, 0f);
    }
}