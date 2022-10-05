using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;
    private Keyboard keyboard;
    public float speed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void FixedUpdate()
    {
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            characterController.Move(Vector3.forward*speed);
        }
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            characterController.Move(Vector3.left*speed);
        }
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            characterController.Move(Vector3.right*speed);
        }
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            characterController.Move(Vector3.back*speed);
        }
    }
}
