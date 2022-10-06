using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    private CharacterController characterController;
    private Keyboard keyboard;
    public float speed;
    private Vector3 moveDirection;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            MovePlayer();
        }
        
        if (IsClient)
        {
            if (IsLocalPlayer)
            {
                if (!keyboard.anyKey.isPressed)
                {
                    RequestMoveCancelServerRpc();
                }
                
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                {
                    RequestMoveServerRpc(Vector3.forward);
                }
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                {
                    RequestMoveServerRpc(Vector3.left);
                }
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                {
                    RequestMoveServerRpc(Vector3.right);
                }
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                {
                    RequestMoveServerRpc(Vector3.back);
                }
            }
        }
    }

    private void MovePlayer()
    {
        characterController.Move(moveDirection * speed);
    }

    [ServerRpc]
    private void RequestMoveServerRpc(Vector3 direction)
    {
        moveDirection = direction;
    }

    [ServerRpc]
    private void RequestMoveCancelServerRpc()
    {
        moveDirection = Vector3.zero;
    }
}
