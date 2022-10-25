using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Conditions;
using Unity.VisualScripting;
using UnityEngine;

namespace Lloyd
{
    public class DoorModel : MonoBehaviour, IFlammable
    {
        //what door am I globally?
        [SerializeField] private int _mainDoorInt=1;
        
        //State Manager
        public MonoBehaviour _moveState;
        public MonoBehaviour _currentState;

        public MonoBehaviour _idleState;
        private MonoBehaviour _destroyedState;

        //determines if door can be Interacted with
        private bool _isActive = true;

        [SerializeField]private bool _isOpen;

        //Door Health
        [SerializeField] private float _HP;
        private DoorComponents _doorComp01;
        private DoorComponents _doorComp02;

        //Door Fire Damge
        [SerializeField] private float _fireDamage;

        private bool isBurning;

        //Door Movement
        private Vector3 _doorWingPos;
        [SerializeField] private float _speed;

        //Door Wing GameObjects
        [SerializeField] private float _doorWingSize;

        public GameObject _doorWingprefab;
        private GameObject _doorWing01;
        private GameObject _doorWing02;

        void OnEnable()
        {
            EventManager.ChangeHealthEvent += ChangeHP;
            EventManager.DoorIdleEvent += DoorIdle;
            EventManager.DoorInteractedEvent += DoorMove;

            SpawnDoors();
        }
        
        private void DoorMove()
        {
            _isActive = false;
            ChangeState(_moveState);
        }

        private void Start()
        {
            ChangeState(_idleState);
        }

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

            _doorWing01 = Instantiate(_doorWingprefab, _doorWingPos, Quaternion.identity);

            _doorWing01.name = "Door01";

            _doorComp01 = _doorWing01.GetComponent<DoorComponents>();
            
            _doorComp01.SetDoorComps(_mainDoorInt, 1, _HP, _fireDamage, _speed);
            
            

            _doorWingPos = new Vector3(transform.position.x + (_doorWingSize), transform.position.y,
                transform.position.z);

            _doorWing02 = Instantiate(_doorWingprefab, _doorWingPos, Quaternion.identity);

            _doorWing02.name = "Door02";
            
            _doorComp02 = _doorWing01.GetComponent<DoorComponents>();

            _doorComp02.SetDoorComps(_mainDoorInt, 2, _HP, _fireDamage, _speed);
        }



        public void Interact()
        {
            if (_isActive)
            {
                _isOpen = !_isOpen;
                
                ChangeState(_moveState);
                EventManager.DoorIdleFunction();

                _isActive = false;
            }
        }

        private void DoorIdle()
        {
            _isActive = true;
            ChangeState(_idleState);
            Debug.Log("whee");
        }

        /*private void DoorMove()
        {
            _isActive = false;
            ChangeState(_moveState);
        }*/

        private void FixedUpdate()
        {
            if (isBurning)
            {
                EventManager.ChangeHealthFunction(-_fireDamage);
            }
        }


        public void SetOnFire()
        {
            EventManager.BurningEventFunction();
            isBurning = true;
        }

        public void Burnt()
        {
            EventManager.BurntEventFunction();
            _HP = 0;
        }


        private void OnDisable()
        {
            EventManager.ChangeHealthEvent -= ChangeHP;
            EventManager.DoorInteractedEvent -= DoorMove;
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

        public bool IsActive()
        {
            return _isActive;
        }
    }
}