using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
        private InputAction _action5;

        [SerializeField] private Vector2 _moveDirection;
        private Vector2 _aimDirection;

        private void OnEnable()
        {
            playerControls = new PlayerControls();
            playerControls.Player.Enable();
            _move = playerControls.Player.Move;
            _aim = playerControls.Player.Aim;
            _action1 = playerControls.Player.Action1PickupDrop;
            _action2 = playerControls.Player.Action2Camera;
            _action3 = playerControls.Player.Action3ExternalUse;
            _action4 = playerControls.Player.Action4UseFire;
            _action5 = playerControls.Player.Action5AltuseFirealt;

            _move.performed += MovePerformed;
            _move.canceled += MoveCancelled;

            _aim.performed += AimPerformed;
            _aim.canceled += AimCancelled;

            _action1.performed += Action1PickupDropPerformed;

            _action2.performed += Action2CameraPerformed;

            _action3.performed += Action3ExternalUsePerformed;
            
            _action4.performed += Action4UseFirePerformed;
            _action4.canceled += Action4UseFireCancelled;
            
            _action5.performed += Action5AltUseFirePerformed;
            _action5.canceled += Action5AltUseFireCancelled;
        }

        private void OnDisable()
        {
            _move.performed -= MovePerformed;
            _move.canceled -= MoveCancelled;

            _aim.performed -= AimPerformed;
            _aim.canceled -= AimCancelled;

            _action1.performed -= Action1PickupDropPerformed;

            _action2.performed -= Action2CameraPerformed;

            _action3.performed -= Action3ExternalUsePerformed;

            _action4.performed -= Action4UseFirePerformed;
            _action4.canceled -= Action4UseFireCancelled;

            _action5.performed -= Action5AltUseFirePerformed;
            _action5.canceled -= Action5AltUseFireCancelled;
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
                Camera cam = GameManager.singleton.cameraBrain.OutputCamera;
                if (cam == null) return;
                Vector2 playerPos = new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f); //Camera.main.WorldToScreenPoint(playerTransform.position);
                _aimDirection = aimInput - playerPos;

                return;
            }
            _aimDirection = aimInput;
        }

        private void AimCancelled(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            _aimDirection = Vector2.zero;
        }

        private void Action1PickupDropPerformed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action1();
        }

        private void Action2CameraPerformed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action2();
        }

        private void Action3ExternalUsePerformed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action3();
        }
        
        private void Action4UseFirePerformed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action4Performed();
        }

        private void Action4UseFireCancelled(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action4Cancelled();
        }
        
        private void Action5AltUseFirePerformed(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action5Performed();
        }
        
        private void Action5AltUseFireCancelled(InputAction.CallbackContext context)
        {
            if (!IsLocalPlayer) return;
            if (player == null) return;
            player.GetComponent<IControllable>()?.Action5Cancelled();
        }

        [ClientRpc]
        public void EnableControlsClientRpc()
        {
	        playerControls.Player.Enable();
        }
        
        [ClientRpc]
        public void DisableControlsClientRpc()
        {
	        playerControls.Player.Disable();
        }
    }
}