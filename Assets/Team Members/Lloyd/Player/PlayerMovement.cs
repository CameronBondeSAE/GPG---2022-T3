using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Lloyd
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody _rb;

        private LloydPlayer _playerInput;

        //put in equip different weapons logic later
        public Flamethrower _flamethrower;

        public LayerMask _floorLayer;

        private bool _isShooting;

        private Vector2 _movement;

        [SerializeField] private float _runSpeed;
        [SerializeField] private float _maxSpeed;

        private Vector2 _mousePos;

        private Vector3 _rotation;
        
        private void OnEnable()
        {
            _rb = GetComponent<Rigidbody>();
            _playerInput = new LloydPlayer();

            // _playerInput.Player.Fire.performed += Fire => _isShooting = true;
            //  _playerInput.Player.Fire.canceled += Fire => _isShooting = false;

            _playerInput.Player.Fire.performed += Fire;
            _playerInput.Player.Fire.canceled += Fire;

            _playerInput.Player.Fire.performed += AltFire;

            _playerInput.Player.Enable();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            Look();

            if (_isShooting)
                _flamethrower.ShootFire();
        }


        //LOOK
        public void MouseLook(InputAction.CallbackContext context)
        {
            _mousePos = _playerInput.Player.MousePosition.ReadValue<Vector2>();
        }

        private void Rotate(Vector3 rotation)
        {
            transform.LookAt(rotation);
        }

        private void Look()
        {
            MouseLook(new InputAction.CallbackContext());

            Ray ray = Camera.main.ScreenPointToRay(_mousePos);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, _floorLayer))
            {
                _rotation = new Vector3(raycastHit.point.x, _rb.transform.position.y, raycastHit.point.z);
                Rotate(_rotation);
            }
            //Debug.Log(_mousePos);
        }

        //MOVE
        private void HandleMovement()
        {
            _movement = _playerInput.Player.Move.ReadValue<Vector2>();

            _rb.velocity = new Vector3(_movement.x * _runSpeed, 0f, _movement.y * _runSpeed);

            if (_rb.velocity.magnitude > _maxSpeed)
            {
                _rb.velocity = _rb.velocity.normalized * _maxSpeed;
            }
        }

        //SHOOT
        private void Fire(InputAction.CallbackContext context)
        {
            if (context.performed)
                _isShooting = true;
            if (context.canceled)
                _isShooting = false;
        }

        private void AltFire(InputAction.CallbackContext context)
        {
            _flamethrower.ShootAltFire();
        }
    }
}