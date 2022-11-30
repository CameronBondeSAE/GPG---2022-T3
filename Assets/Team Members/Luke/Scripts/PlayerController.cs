using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Luke
{
    public class PlayerController : NetworkBehaviour
    {
        public GameObject player;
        public Transform playerTransform;

        public PlayerControls playerControls;
        private InputAction _move;
        private InputAction _aim;
        private InputAction _action1;
        private InputAction _action2;
        private InputAction _action3;
        private InputAction _action4;

        [SerializeField] private Vector2 _moveDirection;
        private Vector2 _aimDirection;

        private void OnEnable()
        {
            playerControls = new PlayerControls();
            playerControls.Player.Enable();
            _move = playerControls.Player.Move;
            _aim = playerControls.Player.Aim;
            _action1 = playerControls.Player.Action1;
            _action2 = playerControls.Player.Action2;
            _action3 = playerControls.Player.Action3;
            _action4 = playerControls.Player.Action4;

            _move.performed += MovePerformed;
            _move.canceled += MoveCancelled;

            _aim.performed += AimPerformed;
            _aim.canceled += AimCancelled;

            _action1.performed += Action1Performed;

            _action2.performed += Action2Performed;

            _action3.performed += Action3Performed;
            
            _action4.performed += Action4Performed;
        }

        private void OnDisable()
        {
            _move.performed -= MovePerformed;
            _move.canceled -= MoveCancelled;

            _aim.performed -= AimPerformed;
            _aim.canceled -= AimCancelled;

            _action1.performed -= Action1Performed;

            _action2.performed -= Action2Performed;

            _action3.performed -= Action3Performed;
            
            _action4.performed -= Action4Performed;
        }

        private void FixedUpdate()
        {
            if (player == null) return;
            player.GetComponent<IControllable>()?.Move(_moveDirection);
            player.GetComponent<IControllable>()?.Aim(_aimDirection);
        }

        private void MovePerformed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            _moveDirection = context.ReadValue<Vector2>();
        }

        private void MoveCancelled(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            _moveDirection = Vector2.zero;
        }

        private void AimPerformed(InputAction.CallbackContext context)
        {
	        if (!IsLocalPlayer) return;
            if (player == null) return;
            Vector2 aimInput = context.ReadValue<Vector2>();
            if (context.control.parent.name == "Mouse")
            {
                if (Camera.main == null) return;
                Vector3 playerPos = Camera.main.WorldToScreenPoint(playerTransform.position);
                _aimDirection = aimInput - new Vector2(playerPos.x, playerPos.y);
                return;
            }
            _aimDirection = aimInput;
        }

        private void AimCancelled(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            _aimDirection = Vector2.zero;
        }

        private void Action1Performed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action1();
        }

        private void Action2Performed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action2();
        }

        private void Action3Performed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action3();
        }
        
        private void Action4Performed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action4();
        }
    }
}