using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwarmPlayer : MonoBehaviour
{
    private Flamethrower _flamethrower;
    
    //private PlayerActions _playerActions;

    private Rigidbody _rb;

    private void OnEnable()
    {
       // _playerActions = new PlayerActions();
    }
    
    // // // // // //
    // UPDATE
    //
    private void Update()
    {
       // _movement = _playerActions.InGamePlayer.Movement.ReadValue<Vector2>();

       // _aimVector = _playerActions.InGamePlayer.AimVector.ReadValue<Vector2>();

        //  Debug.Log(_movement);
    }

    private void FixedUpdate()
    {
        HandleMovement();

    //    HandleGravity();
    }
    
    // // // // // //
    // Running
    //
    private void HandleMovement()
    {
        /*_rb.velocity = new Vector3(_movement.x * _runSpeed, 0f,0f);
        
        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
        }*/
    }

    // // // // // //
    // ATTACKING
    //
    public void Attack(InputAction.CallbackContext context)
    {
    }



}
