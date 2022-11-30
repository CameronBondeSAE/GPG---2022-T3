using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;


/// <summary>
/// Put this on the Player
/// allows them to pickup plants & items
/// also to open doors and use items
/// </summary>
public class Interact : NetworkBehaviour
{
    public FlamethrowerModel heldObject;
    public GameObject clientFlamethrowerModel;
    public Transform equippedMountPos;
    [Serialize] public IPickupable pickupableNearby;
    public int storedItems = 5;
    public int storedMax = 10;
    public int equippedItems;
    public int equippedMax = 1;

    [Header("Hack Item Spawning")]
    public NetworkObject item;
    public NetworkObject flamethrower;
    public NetworkObject plant;

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftAlt))
        // {
        //     RequestDropItemServerRpc();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     RequestUseItemServerRpc();
        // }
    }

    [ServerRpc]
    public void RequestPickUpItemServerRpc()
    {
        if (pickupableNearby != null)
        {
            if (equippedItems < equippedMax)
            {
                pickupableNearby.PickedUp(gameObject);
                
                MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
                if (monoBehaviour != null)
                {
	                // monoBehaviour.transform.parent = equippedMountPos;
	                Debug.Log("TrySetParent = "+ monoBehaviour.GetComponent<NetworkObject>().TrySetParent(GetComponent<NetworkObject>(), false));
                    PickUpItemClientRpc();
                    monoBehaviour.GetComponent<Transform>().localPosition = new Vector3(0,1,0);
                    monoBehaviour.GetComponent<Transform>().rotation = transform.rotation;
                    heldObject = GetComponentInChildren<FlamethrowerModel>();
                    equippedItems++;
                }
                pickupableNearby = null;
            }
        }
    }
    
    [ClientRpc]
    public void PickUpItemClientRpc()
    {
        if(!IsServer) clientFlamethrowerModel.SetActive(true);
    }

    [ServerRpc]
    public void RequestDropItemServerRpc()
    {
        if (heldObject != null)
        {
            Destroy(heldObject.gameObject);
            Vector3 myPos = transform.position;
            NetworkObject go = Instantiate(flamethrower);
            go.transform.position = myPos + transform.forward;
            go.transform.rotation = transform.rotation;
            //go.Spawn();
            equippedItems = 0;
            heldObject = null;
        }
    }
    
    [ClientRpc]
    public void DropItemClientRpc()
    {
        if(!IsServer) clientFlamethrowerModel.SetActive(false);
    }

    [ServerRpc]
    public void RequestUseItemServerRpc()
    {
        //if you've got a flamethrower, fire it
        if (heldObject != null)
        {
            IInteractable interactable = heldObject.GetComponent<IInteractable>();
            if(interactable!=null) interactable.Interact(this.gameObject);
        }
    }

    [ClientRpc]
    public void UseItemClientRpc()
    {
        //client doesn't have a heldObject to interact with
        //network the results of interact
        //eg, door open, fireballs shot out, etc
    }
    
    [ServerRpc]
    public void RequestExternalUseItemServerRpc()
    {
	    //there's one nearby, fire that on the floor
        if (pickupableNearby != null && pickupableNearby.isHeld == false)
        {
	        MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
	        if (monoBehaviour != null && monoBehaviour.GetComponent<IInteractable>() != null)
	        {
		        monoBehaviour.GetComponent<IInteractable>().Interact(this.gameObject);
	        }
        }
    }

    [ClientRpc]
    public void ExternalUseItemClientRpc()
    {
        //client doesn't have a heldObject to interact with
        //network the results of interact
        //eg, door open, fireballs shot out, etc
    }

    [ServerRpc]
    public void RequestUseAltItemServerRpc()
    {
        //if you've got a flamethrower, fire it
        //if you don't have one AND there's one nearby, fire that on the floor
        if (heldObject != null)
        {
            FlamethrowerModel flamethrower = heldObject.GetComponent<FlamethrowerModel>();
            if(flamethrower!=null) flamethrower.ShootAltFire();
        }
        else if (pickupableNearby != null && pickupableNearby.isHeld == false)
        {
            MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
            if (monoBehaviour != null && monoBehaviour.GetComponent<FlamethrowerModel>() != null)
            {
                monoBehaviour.GetComponent<FlamethrowerModel>().ShootAltFire();
            }
        }
    }

    [ClientRpc]
    public void UseAltItemClientRpc()
    {
        //client doesn't have a heldObject to interact with
        //network the results of interact
        //eg, door open, fireballs shot out, etc
    }
    
    public void DeathItemRespawn()
    {
        StartCoroutine(DeathItemRespawnCoroutine());
    }

    public IEnumerator DeathItemRespawnCoroutine()
    {
        if (storedItems > 0)
        {
            storedItems--;
            Vector3 myPos = transform.position;
            float randomX = (UnityEngine.Random.Range(-10, 10));
            float randomY = (UnityEngine.Random.Range(0, 0));
            float randomZ = (UnityEngine.Random.Range(-10, 10));
            Vector3 randomForce = new Vector3(randomX, randomY, randomZ);
            NetworkObject go = Instantiate(plant);
            go.Spawn();
            go.transform.position =
                myPos + transform.up;
            Rigidbody rb = go.GetComponent<Rigidbody>();
        
            //TODO: Coolness
            //fuck with the constraints and the timer to make it look cooler
            RigidbodyConstraints tempConstraints = rb.constraints;
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(randomForce,ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(DeathItemRespawnCoroutine());
            yield return new WaitForSeconds(2);
            rb.constraints = tempConstraints;
        }
        else
        {
            storedItems = 0;
        }
    }

    public void ParentItemObject(GameObject child)
    {
        //child.transform.parent = equippedMountPos;
    }

    private void OnTriggerEnter(Collider other)
    {
	    IPickupable pickupable = other.GetComponent<IPickupable>();
	    if (pickupable != null)
        {
	        Debug.Log("Pickupable in range : "+pickupable);
            IPickupable item = pickupable;
            if (item.autoPickup)
            {
                if (storedItems < storedMax)
                {
                    item.PickedUp(gameObject);
                    item.DestroySelf();
                    storedItems++;
                }
                else
                {
                    print("You've got too many plants!");
                }
            }
            else //ie, not a plant
            {
                pickupableNearby = item;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPickupable>() != null)
        {
            pickupableNearby = null;
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (IsServer)
        {
            if (other.GetComponent<IPickupable>() != null)
            {
                if (storedItems == 0)
                {
                    if (!other.gameObject.GetComponent<ItemBase>().locked)
                    {
                        other.transform.parent.gameObject.SetActive(false);
                        heldObject = other.gameObject;
                        storedItems++;
                        heldObject.GetComponent<ItemBase>().GetPickedUpClientRpc();
                    }
                }
                else print("You're already holding an item! Press space to drop it.");
            }
            
        }
    }*/
}
