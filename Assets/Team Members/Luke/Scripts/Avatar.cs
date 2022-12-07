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
    private void RequestMovePlayerServerRpc(Vector2 direction)
    {
        MovePlayer(direction);
    }
    
    [ServerRpc]
    private void RequestAimPlayerServerRpc(Vector2 direction)
    {
	    AimPlayer(direction);
    }

    private void MovePlayer(Vector2 direction)
    {
        // Vector3 velocity = _rb.velocity;
        // Vector3 dragVector3 = new Vector3(-velocity.x*drag, 0, -velocity.z*drag);
        // _rb.AddForce(dragVector3, ForceMode.Acceleration);
        // Vector3 heading = new (direction.x*acceleration, 0,direction.y*acceleration);
        // _rb.AddForce(heading, ForceMode.Acceleration);
        // _rb.velocity = new(Mathf.Clamp(velocity.x,-maxSpeed, maxSpeed),velocity.y,
        //     Mathf.Clamp(velocity.z,-maxSpeed, maxSpeed));
        // account for direction player facing here

        _rb.velocity = new Vector3(direction.x, 0, direction.y) * acceleration;
    }

    private void AimPlayer(Vector2 direction)
    {
	    Vector3 targetAngle = new Vector3(0,turnSpeed*Vector3.SignedAngle(_transform.forward, new Vector3(direction.x, 0, direction.y), Vector3.up),0);
	    _rb.AddTorque(targetAngle, ForceMode.Acceleration);
    }

    #region IControllable Interface

    public void Move(Vector2 direction)
    {
	    if (!IsOwner) return;
        MovePlayer(direction); // Clientside prediction
        RequestMovePlayerServerRpc(direction);
    }

    public void Aim(Vector2 direction)
    {
	    if (!IsOwner) return;
        AimPlayer(direction); // Clientside prediction
        RequestAimPlayerServerRpc(direction);
    }

    public void Action1()
    {
	    //does this work? refactored from heldObject being hardcoded as Flamethrower
        if (interact.heldObject != null || interact.clientHeldObject)
	    {
		    interact.RequestDropItemServerRpc(NetworkObjectId);
            print("trying to drop");
	    }
	    else
	    {
		    interact.RequestPickUpItemServerRpc(NetworkObjectId);
        }
    }

    public void Action2()
    {
        if (GameManager.singleton.zoomedIn)
        {
            GameManager.singleton.virtualCameraTwo.gameObject.SetActive(true);
            GameManager.singleton.zoomedIn = false;
            return;
        }
        GameManager.singleton.virtualCameraTwo.gameObject.SetActive(false);
        GameManager.singleton.zoomedIn = true;
    }
    
    public void Action3()
    {
	    interact.RequestExternalUseItemServerRpc();
    }
    
    public void Action4Performed()
    {
        interact.RequestUseItemServerRpc();
    }

    public void Action4Cancelled()
    {
        
    }

    public void Action5Performed()
    {
	    interact.RequestUseAltItemServerRpc();
    }

    public void Action5Cancelled()
    {
        interact.RequestUseAltItemCancelServerRpc();
    }

    #endregion
}
