using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Conditions;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;

namespace Lloyd
{
    public class DoorModel : MonoBehaviour, IFlammable
    {
        [SerializeField]
        enum DoorType
        {
            DoorSingle,
            DoorDouble,
            DoorAlien
        };

        private DoorType _myType;
        
        //what door am I globally?
        [SerializeField] private int _mainDoorInt=1;
        
        //State Manager
        private MonoBehaviour _currentState;
        
        private MonoBehaviour _moveState;
        private MonoBehaviour _idleState;
        private MonoBehaviour _destroyedState;

        //determines if door can be Interacted with
        private bool _isActive = true;

        //tracks if door is open or closed
       [SerializeField] private bool _isOpen;

        //Door Health
        [SerializeField] private float _HP;
     
        //Door Fire Damge
        [SerializeField] private float _fireDamage;
        private bool isBurning;

        //Door Movement
        private Vector3 _doorWingPos;
        [SerializeField] private float _speed;

        [SerializeField] private int _timeMoving;

        //Door Wing GameObjects
        [SerializeField] private float _doorWingSize;

        [SerializeField] private float _numDoors;
        public List<GameObject> _doorList = new List<GameObject>();

        public GameObject _doorWingprefab;
        private GameObject _doorWing01;
        private GameObject _doorWing02;

        private DoorComponents _doorComp;
        private DoorComponents _doorComp01;
        private DoorComponents _doorComp02;

        public DoorEventManager _doorEvent;
        
        void OnEnable()
        {
            _doorEvent.ChangeHealthEvent += ChangeHP;
            _doorEvent.DoorIdleEvent += DoorIdle;
            _doorEvent.DoorMoveEvent += DoorMove;

            _moveState = GetComponent<DoorMovingState>();
            _idleState = GetComponent<DoorIdleState>();

            ChangeState(_idleState); 
            
            SpawnDoors();
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

        private void SpawnDoors()
        {
            _doorWingPos = new Vector3(transform.position.x - (_doorWingSize), transform.position.y,
                transform.position.z);

            _doorWing01 = Instantiate(_doorWingprefab, _doorWingPos, Quaternion.Euler(-90, 0, 0));

            _doorWing01.name = "Door01";

            _doorComp01 = _doorWing01.GetComponent<DoorComponents>();
            
            _doorComp01.SetDoorComps(_mainDoorInt, 1, _HP, _fireDamage, _speed);

            _doorWing01.transform.SetParent(this.transform,false);

            _doorWingPos = new Vector3(transform.position.x + (_doorWingSize), transform.position.y,
                transform.position.z);

            _doorWing02 = Instantiate(_doorWingprefab, _doorWingPos, Quaternion.Euler(-90, 0, 0));

            _doorWing02.name = "Door02";
            
            _doorComp02 = _doorWing02.GetComponent<DoorComponents>();

            _doorComp02.SetDoorComps(_mainDoorInt, 2, _HP, _fireDamage, _speed);
            
            _doorWing02.transform.SetParent(this.transform, false);
        }

        public void Interact()
        {
            if (_isActive)
            {
                EventManager.singleton.DoorInteractedFunction();

                _isOpen = !_isOpen;
                
                ChangeState(_moveState);

                _isActive = false;
            }
        }

        private void DoorIdle()
        {
            _isActive = true;
            ChangeState(_idleState);
        }

        private void FixedUpdate()
        {
            if (isBurning)
            {
                EventManager.singleton.ChangeHealthFunction(-_fireDamage);
            }
        }

        public void SetOnFire()
        {
            EventManager.singleton.BurningEventFunction();
            isBurning = true;
        }

        public void Burnt()
        {
            EventManager.singleton.BurntEventFunction();
            _HP = 0;
        }
        
        private void OnDisable()
        {
            EventManager.singleton.ChangeHealthEvent -= ChangeHP;
            EventManager.singleton.DoorIdleEvent -= DoorIdle;
            EventManager.singleton.DoorMoveEvent -= DoorMove;
        }

        private void ChangeHP(float amount)
        {
            _HP += amount;

            if (_HP <= 0)
                Burnt();
        }
        
        public GameObject Wing01()
        {
            return _doorWing01;
        }

        public GameObject Wing02()
        {
            return _doorWing02;
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