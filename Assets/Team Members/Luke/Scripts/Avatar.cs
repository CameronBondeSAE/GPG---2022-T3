using System.Collections;
using System.Collections.Generic;
using Luke;
using Tanks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : NetworkBehaviour, IControllable
{
    private Vector2 _moveInput;
    private Interact interact;
    private Transform _transform;
    private Rigidbody _rb;
    public float acceleration = 100f;
    public float maxSpeed = 15f;
    public float turnSpeed = 5f;
    public float drag = 7f;

    private void OnEnable()
    {
	    _transform = transform;
        _rb = GetComponent<Rigidbody>();
        interact = GetComponent<Interact>();
    }

    [ServerRpc]
    private void RequestPlayerFixedUpdateServerRpc(Vector2 input)
    {
        MovePlayer(input);
    }

    private void MovePlayer(Vector2 input)
    {
        Vector3 velocity = _rb.velocity;
        Vector3 dragVector3 = new Vector3(-velocity.x*drag, 0, -velocity.z*drag);
        _rb.AddForce(dragVector3, ForceMode.Acceleration);
        Vector3 heading = new (input.x*acceleration, 0,input.y*acceleration);
        _rb.AddForce(heading, ForceMode.Acceleration);
        _rb.velocity = new(Mathf.Clamp(velocity.x,-maxSpeed, maxSpeed),velocity.y,
            Mathf.Clamp(velocity.z,-maxSpeed, maxSpeed));
        _rb.AddTorque(0f,turnSpeed*Vector3.SignedAngle(_transform.forward,heading,Vector3.up),0f, ForceMode.Acceleration);
        _rb.angularVelocity = Vector3.zero;
    }

    public void Move(Vector2 direction)
    {
        MovePlayer(direction); //Clientside prediction
        RequestPlayerFixedUpdateServerRpc(direction);
    }

    public void Aim(Vector2 direction)
    {
        
    }

    public void Action1()
    {
        interact.RequestInteractWithServerRpc();
    }

    public void Action2()
    {
        if (GameManager.singleton.zoomedIn)
        {
            GameManager.singleton.virtualCameraTwo.SetActive(true);
            GameManager.singleton.zoomedIn = false;
        }
        else if (GameManager.singleton.zoomedIn == false)
        {
            GameManager.singleton.virtualCameraTwo.SetActive(false);
            GameManager.singleton.zoomedIn = true;
        }
    }
    
    public void Action3()
    {
        
    }
}
