using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Luke;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using NetworkObject = Unity.Netcode.NetworkObject;
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
    public bool clientHeldObject = false;

    [Header("Hack Item Spawning")]
    public NetworkObject item;
    public GameObject flamethrower;
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
    public void RequestPickUpItemServerRpc(ulong localClientId)
    {
        if (pickupableNearby == null) return;
        if (equippedItems >= equippedMax) return;
        
        //send through player client id ulong
        pickupableNearby.PickedUp(gameObject, localClientId);

        heldObject = GetComponentInChildren<FlamethrowerModel>();
        PickUpItemClientRpc(localClientId);
        //Old parenting with NetworkObject --- OBSOLETE
        /*MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
        if (monoBehaviour != null)
        {
             monoBehaviour.transform.parent = equippedMountPos;
            NetworkObject monoNetObj = monoBehaviour.GetComponent<NetworkObject>();
            monoNetObj.Despawn();
            Destroy(monoNetObj.gameObject);
            
            
            
             Debug.Log("TrySetParent = "+ monoNetObj.TrySetParent(transform, false));
             if(monoNetObj.TrySetParent(transform, false) == true)
             {
                 PickUpItemClientRpc(monoNetObj.NetworkObjectId);
                 monoBehaviour.GetComponent<Transform>().localPosition = new Vector3(0,1,-1.12f);
                 monoBehaviour.GetComponent<Transform>().rotation = transform.rotation;
                 heldObject = GetComponentInChildren<FlamethrowerModel>();
                 if(heldObject != null) equippedItems++;
             }
            
            
        }*/
        pickupableNearby = null;
    }
    
    [ClientRpc]
    public void PickUpItemClientRpc(ulong localClientId)
    {
        if (!IsServer)
        {
            //clientFlamethrowerModel.SetActive(true);
            clientHeldObject = true;
        }
    }

    [ServerRpc]
    public void RequestDropItemServerRpc(ulong localClientId)
    {
        if (heldObject != null)
        {
            equippedItems = 0;
            heldObject.PutDown(gameObject, localClientId);
            DropItemClientRpc(localClientId);
            
            heldObject = null;
            // Destroy(heldObject.gameObject);
            // heldObject = null;
            // Vector3 myPos = transform.position;
            // GameManager.singleton.NetworkInstantiate(flamethrower, (myPos + (transform.forward / 2)),
            //     transform.rotation);
            
            print("respawn flamethrower");
        }
    }
    
    [ClientRpc]
    public void DropItemClientRpc(ulong localClientId)
    {
        if (!IsServer)
        {
            //clientFlamethrowerModel.SetActive(false);
            clientHeldObject = false;
        }
    }

    [ServerRpc]
    public void RequestUseItemServerRpc()
    {
        //if you've got a flamethrower, fire it
        if (heldObject != null)
        {
            IInteractable interactable = heldObject.GetComponent<IInteractable>();
            if(interactable!=null) interactable.Interact(gameObject);
        }
    }
    
    [ServerRpc]
    public void RequestUseItemCancelServerRpc()
    {
        //if you've got a flamethrower, fire it
        if (heldObject != null)
        {
            IInteractable interactable = heldObject.GetComponent<IInteractable>();
            if(interactable!=null) interactable.CancelInteract();
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
            MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
            if (monoBehaviour != null && monoBehaviour.GetComponent<IInteractable>() != null)
            {
                monoBehaviour.GetComponent<IInteractable>().AltInteract(gameObject);
            }
        }
        
        //alt use on floor should be disabled
        
        // else if (pickupableNearby != null && pickupableNearby.isHeld == false)
        // {
        //     MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
        //     if (monoBehaviour != null && monoBehaviour.GetComponent<FlamethrowerModel>() != null)
        //     {
        //         monoBehaviour.GetComponent<FlamethrowerModel>().AltInteract(gameObject);
        //     }
        // }
    }

    [ServerRpc]
    public void RequestUseAltItemCancelServerRpc()
    {
        MonoBehaviour monoBehaviour = pickupableNearby as MonoBehaviour;
        if (monoBehaviour != null && monoBehaviour.GetComponent<IInteractable>() != null)
        {
            monoBehaviour.GetComponent<IInteractable>().CancelAltInteract();
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
                    item.PickedUp(gameObject,NetworkManager.LocalClientId);
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
