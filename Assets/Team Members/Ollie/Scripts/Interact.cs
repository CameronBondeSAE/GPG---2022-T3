using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lloyd;
using Luke;
using Sirenix.OdinInspector;
using TMPro;
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
    public IPickupable heldObject;
    public GameObject clientFlamethrowerModel;
    public Transform equippedMountPos;
    [Serialize] public IPickupable pickupableNearby;
    public int storedItems = 0;
    public int storedMax = 10;
    public int equippedItems;
    public int equippedMax = 1;
    public bool clientHeldObject = false;

    [Header("Hack Item Spawning")]
    public NetworkObject item;
    public GameObject flamethrower;
    public GameObject plant;

    [Header("Plants Counter")]
    public TMP_Text scoreText;
    public GameWaveTimer gameWaveTimer;

    private Health playerHealth = null; //Luke HACK

    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();

	    if (!IsServer) return;
	    if(GetComponent<Avatar>() != null) playerHealth = GetComponent<Health>();
    }

    private void Start()
    {
        gameWaveTimer = FindObjectOfType<GameWaveTimer>();
    }

    [ServerRpc]
    public void RequestPickUpItemServerRpc(ulong networkObjectId)
    {
        if (pickupableNearby == null) return;
        if (equippedItems >= equippedMax) return;
        
        //send through player client id ulong
        pickupableNearby.PickedUp(gameObject, networkObjectId);

        heldObject = GetComponentInChildren<IPickupable>();
        PickUpItemClientRpc(networkObjectId);
        
        pickupableNearby = null;
    }
    
    [ClientRpc]
    public void PickUpItemClientRpc(ulong networkObjectId)
    {
        if (!IsServer)
        {
            //clientFlamethrowerModel.SetActive(true);
            clientHeldObject = true;
        }
    }

    [ServerRpc]
    public void RequestDropItemServerRpc(ulong networkObjectId)
    {
        if (heldObject != null)
        {
            equippedItems = 0;
            heldObject.PutDown(gameObject, networkObjectId);
            DropItemClientRpc(networkObjectId);
            
            heldObject = null;
        }
    }
    
    [ClientRpc]
    public void DropItemClientRpc(ulong networkObjectId)
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
            MonoBehaviour monoBehaviour = heldObject as MonoBehaviour;
            if (monoBehaviour != null)
            {
                IInteractable interactable = monoBehaviour.GetComponent<IInteractable>();
                if(interactable!=null) interactable.Interact(gameObject);
            }
        }
    }
    
    [ServerRpc]
    public void RequestUseItemCancelServerRpc()
    {
        //if you've got a flamethrower, fire it
        if (heldObject != null)
        {
            MonoBehaviour monoBehaviour = heldObject as MonoBehaviour;
            if (monoBehaviour != null)
            {
                IInteractable interactable = monoBehaviour.GetComponent<IInteractable>();
                if(interactable!=null) interactable.CancelInteract();
            }
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

    public void IncreaseHeadScore()
    {
        IncreaseHeadScoreClientRpc(storedItems);
    }

    [ClientRpc]
    public void IncreaseHeadScoreClientRpc(int score)
    {
        if (score < storedMax)
        {
            scoreText.color = Color.white;
        }
        if(scoreText!=null) scoreText.text = (score.ToString() + " / " +storedMax.ToString());
        if (storedItems >= storedMax)
        {
            scoreText.color = Color.red;
            scoreText.transform.DOPunchScale((Vector3.one) * 2, 0.1f, 2, 0f);
        }
        scoreText.transform.localScale = Vector3.one;
    }
    
    public void ResetHeadScore()
    {
        ResetHeadScoreClientRpc();
    }

    [ClientRpc]
    public void ResetHeadScoreClientRpc()
    {
        storedItems = 0;
        scoreText.color = Color.white;
        if(scoreText!=null) scoreText.text = (storedItems.ToString() + "/" + storedMax.ToString());
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
            IncreaseHeadScore();
            Vector3 myPos = transform.position;
            float randomX = (UnityEngine.Random.Range(-10, 10));
            float randomY = (UnityEngine.Random.Range(0, 0));
            float randomZ = (UnityEngine.Random.Range(-10, 10));
            Vector3 randomForce = new Vector3(randomX, randomY, randomZ);
            GameObject go = GameManager.singleton.NetworkInstantiate(plant, myPos + transform.up*3, Quaternion.identity);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            
            //TODO: Coolness
            //fuck with the constraints and the timer to make it look cooler
            rb.AddForce(randomForce,ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(DeathItemRespawnCoroutine());
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
	    if (!IsServer) return;
	    if (playerHealth != null)
	    {
		    if (!playerHealth.isAlive) return;
	    }
	    IPickupable pickupable = other.GetComponent<IPickupable>();
	    if (pickupable != null && NetworkManager.Singleton != null)
        {
            IPickupable item = pickupable;
            if (item.autoPickup && storedItems < storedMax)
            {
                item.PickedUp(gameObject,NetworkManager.LocalClientId);
                item.DestroySelf();
                storedItems++;
                IncreaseHeadScore();
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
