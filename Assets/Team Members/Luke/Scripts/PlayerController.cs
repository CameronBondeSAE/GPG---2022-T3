using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public GameObject player;
    public Transform playerTransform;

    private PlayerControls _playerControls;
    private InputAction _move;
    private InputAction _aim;
    private InputAction _action3;
    private InputAction _action1;
    private InputAction _action2;

    [SerializeField] private Vector2 _moveDirection;
    private Vector2 _aimInput;
    private Vector2 _aimDirection;
    
    private void OnEnable()
    {
	    _playerControls = new PlayerControls();
	    _move = _playerControls.Player.Move;
	    _aim = _playerControls.Player.Aim;
	    _action1 = _playerControls.Player.Action1;
	    _action2 = _playerControls.Player.Action2;
        _action3 = _playerControls.Player.Action3;

	    _move.Enable();
	    _move.performed += MovePerformed;
	    _move.canceled += MoveCancelled;
	    
	    _aim.Enable();
	    _aim.performed += AimPerformed;
	    _aim.canceled += AimCancelled;

        _action1.Enable();
	    _action1.performed += Action1Performed;
	    
	    _action2.Enable();
	    _action2.performed += Action2Performed;
        
        _action3.Enable();
        _action3.performed += Action3Performed;
    }

    private void OnDisable()
    {
	    _move.performed -= MovePerformed;
	    _move.canceled -= MoveCancelled;
	    _move.Disable();

	    _aim.performed -= AimPerformed;
	    _aim.canceled -= AimCancelled;
	    _aim.Disable();

        _action1.performed -= Action1Performed;
	    _action1.Disable();
	    
	    _action2.performed -= Action2Performed;
	    _action2.Disable();
        
        _action3.performed -= Action3Performed;
        _action3.Disable();
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
        if (playerTransform == null) return;
	    _aimInput = context.ReadValue<Vector2>();
	    if (context.control.parent.name == "Mouse")
	    {
		    if (Camera.main == null) return;
		    Vector3 playerPos = Camera.main.WorldToScreenPoint(playerTransform.position);
		     _aimDirection = Vector2.ClampMagnitude(_aimInput-new Vector2(playerPos.x, playerPos.y), 1f);
		    return;
	    }

	    _aimDirection = _aimInput;
    }
    
    private void AimCancelled(InputAction.CallbackContext context)
    {
	    if (!IsLocalPlayer) return;
	    _aimInput = Vector2.zero;
        _aimDirection = Vector2.zero;
    }

    private void Action1Performed(InputAction.CallbackContext context)
    {
	    if (!IsLocalPlayer) return;
	    player.GetComponent<IControllable>()?.Action1();
    }
    
    private void Action2Performed(InputAction.CallbackContext context)
    {
	    if (!IsLocalPlayer) return;
	    player.GetComponent<IControllable>()?.Action2();
    }
    
    private void Action3Performed(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer) return;
        player.GetComponent<IControllable>()?.Action3();
    }
}
