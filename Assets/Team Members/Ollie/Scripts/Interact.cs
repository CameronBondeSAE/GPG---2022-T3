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
    public NetworkObject heldObject;
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

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.LeftAlt))
    //     {
    //         RequestUseItemServerRpc();
    //     }
    // }

    [ServerRpc]
    public void RequestPickUpItemServerRpc()
    {
        if (pickupableNearby != null)
        {
            if (equippedItems < equippedMax)
            {
                equippedItems++;
                pickupableNearby.PickedUp(gameObject);
                // pickupableNearby.DestroySelf();

				// Parent TODO: Network it
                MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
                if (monoBehaviour != null)
                {
	                // monoBehaviour.transform.parent = equippedMountPos;
	                Debug.Log("TrySetParent = "+ monoBehaviour.GetComponent<NetworkObject>().TrySetParent(GetComponent<NetworkObject>(), false));
                    monoBehaviour.GetComponent<Transform>().localPosition = new Vector3(0,1,0);
                    heldObject = monoBehaviour as NetworkObject;
                }
                pickupableNearby = null;
            }
        }
    }

    [ServerRpc]
    public void RequestDropItemServerRpc()
    {
        //TODO: need to somehow DROP item
        
        // if (equippedItems >= equippedMax)
        // {
        //     equippedItems--;
        //     Vector3 myPos = transform.position;
        //     NetworkObject go = Instantiate(flamethrower);
        //     go.transform.position =
        //         myPos - transform.forward;
        //     go.Spawn();
        // }
        
        // Vector3 dropPoint = transform.position + transform.forward * 2;
        // if (storedItems > 0 && heldObject != null)
        // {
        //     storedItems--;
        //     heldObject.GetComponent<ItemBase>().GetDropPointClientRpc(dropPoint);
        //     heldObject.GetComponent<ItemBase>().GetDroppedClientRpc();
        // }
    }

    [ServerRpc]
    public void RequestUseItemServerRpc()
    {
        heldObject.GetComponent<FlamethrowerModel>().Interact(this.gameObject);
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
