using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using Tanks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Avatar : NetworkBehaviour, IControllable
{
	public Vector2 moveInput;
    private Interact interact;
    private Transform _transform;
    private Rigidbody _rb;
    public float acceleration = 100f;
    // public float maxSpeed = 15f;
    // public float turnSpeed = 5f;
    // public float drag = 7f;
    public TMP_Text nameText;

    private HatRandomiser hats;

    private void OnEnable()
    {
	    _transform = transform;
        _rb = GetComponent<Rigidbody>();
        interact = GetComponent<Interact>();
        GetComponent<Health>().YouDied += Die;
        hats = GetComponentInChildren<HatRandomiser>();
    }

    private void Die(GameObject go)
    {
	    if (interact.heldObject != null || interact.clientHeldObject)
	    {
		    interact.DropItem(NetworkObjectId);
	    }

	    interact.DeathItemRespawn();
	    // Can client RPC death noise/animations here
	    ToggleMeshRenderersClientRpc(false);
    }

    [ClientRpc]
    public void ToggleMeshRenderersClientRpc(bool newState)
    {
	    foreach (MeshRenderer rend in hats.GetComponentsInChildren<MeshRenderer>())
	    {
		    Transform rendTransform = rend.transform;
		    GameObject go = Instantiate(rend.gameObject, rendTransform.position, rendTransform.rotation);
		    go.AddComponent<Rigidbody>();
		    go.AddComponent<BoxCollider>();
		    go.layer = 13;
		    go.GetComponent<Rigidbody>().angularVelocity =
			    new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
		    go.GetComponent<Rigidbody>().velocity =
			    new Vector3(0, Random.Range(1f, 10f), 0);
		    rend.gameObject.SetActive(false);
	    }
	    foreach (SkinnedMeshRenderer rend in GetComponentsInChildren<SkinnedMeshRenderer>())
	    {
		    rend.enabled = newState;
	    }
    }

    public void ActivateHatRandomiser()
    {
	    hats.ActivateHatClientRpc(Random.Range(0, hats.hats.Count));
    }
    
    public void SetName(string name)
    {
        nameText.text = name;
        SetNameClientRpc(nameText.text);
    }

    [ClientRpc]
    public void SetNameClientRpc(string name)
    {
        nameText.text = name;
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
	    moveInput = direction;
        // Vector3 velocity = _rb.velocity;
        // Vector3 dragVector3 = new Vector3(-velocity.x*drag, 0, -velocity.z*drag);
        // _rb.AddForce(dragVector3, ForceMode.Acceleration);
        // Vector3 heading = new (direction.x*acceleration, 0,direction.y*acceleration);
        // _rb.AddForce(heading, ForceMode.Acceleration);
        // _rb.velocity = new(Mathf.Clamp(velocity.x,-maxSpeed, maxSpeed),velocity.y,
        //     Mathf.Clamp(velocity.z,-maxSpeed, maxSpeed));
        // account for direction player facing here

        _rb.velocity = new Vector3(direction.x, _rb.velocity.y, direction.y) * acceleration;
    }

    private void AimPlayer(Vector2 direction)
    {
	    Vector3 targetAngle = new Vector3(0,Vector3.SignedAngle(_transform.forward, new Vector3(direction.x, 0, direction.y), Vector3.up),0);
	    _transform.Rotate(targetAngle);
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
        interact.RequestUseItemCancelServerRpc();
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
