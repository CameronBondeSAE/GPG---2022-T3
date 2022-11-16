using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Conditions;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;
namespace Lloyd
{
  
    public class DoorModel : MonoBehaviour
    {
        
        //what door am I globally?
        //[SerializeField] private int _mainDoorInt=1;
        
        //State Manager
        private MonoBehaviour _currentState;
        
        private MonoBehaviour _moveState;
        private MonoBehaviour _idleState;
        private MonoBehaviour _destroyedState;

        //determines if door can be Interacted with
        private bool _isActive = true;

        //tracks if door is open or closed
       [SerializeField] private bool _isOpen;

        //Door Movement
        private Vector3 _doorWingPos;
        [SerializeField] private float _speed;

        [SerializeField] private int _timeMoving;

        public DoorEventManager _doorEvent;
        
        void OnEnable()
        {
                _moveState = GetComponent<DoorSingleMoveState>();
                _idleState = GetComponent<DoorIdleState>();
                ChangeState(_idleState);
        }
        
        private void DoorMove()
        {
            _isActive = false;
            ChangeState(_moveState);
        }

        //copy pased from Cam's set up
        public void ChangeState(MonoBehaviour newState)
        {
            if (newState == _currentState)
            {
                return;
            }

            if (_currentState != null)
            {
                _currentState.enabled = false;
            }

            newState.enabled = true;

            // New state swap over to incoming state
            _currentState = newState;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IControllable>() != null)
            {
                Interact();
            }
        }


        public void Interact()
        {
                EventManager.singleton.DoorInteractedFunction();
                
                ChangeState(_moveState);
        }

        private void DoorIdle()
        {
            _isActive = true;
            ChangeState(_idleState);
        }
        
        private void OnDisable()
        {
            EventManager.singleton.DoorIdleEvent -= DoorIdle;
            EventManager.singleton.DoorMoveEvent -= DoorMove;
        }
        
        public bool IsOpen()
        {
            return _isOpen;
        }

        public float GetSpeed()
        {
            return _speed;
        }
        
        public int GetTimeMoving()
        {
            return _timeMoving;
        }

        public bool IsActive()
        {
            return _isActive;
        }
    }
}