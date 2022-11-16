using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Speed,
    Power,
    Defend,
    Goal
}

public class ItemBase : NetworkBehaviour, IGoalItem, IPickupable, IFlammable
{
    private NetworkManager networkManager;
    // public NetworkVariable<Vector3> networkPosition;
    // public Transform parentTransform;
    // private Vector3 tempPosition;
    // private Transform tempParentTransform;
    // public BoxCollider[] boxColliders;
    //
    // public ItemType itemType;

    #region SetUps

    private void Awake()
    {
        networkManager = NetworkManager.Singleton;
        networkManager.OnServerStarted += SetUpItem;
        networkManager.OnClientConnectedCallback += SetUpItemClient;
        //networkPosition.OnValueChanged += OnValueChanged;
    }
    private void OnDisable()
    {
        networkManager.OnServerStarted -= SetUpItem;
        networkManager.OnClientConnectedCallback -= SetUpItemClient;
    }
    
    private void Start()
    {
        SetUpItem();
    }
    void OnGUI()
    {
        //HACK: Was for forcing Item's variables to be set with a button
        //wasn't working in start originally, but loading the scene has made this hack obsolete
        
        /*if (GUILayout.Button("Start"))
        {
            if (IsServer)
            {
                parentTransform = transform.parent.GetComponent<Transform>();
                renderer = parentTransform.GetComponentInChildren<Renderer>();
                rigidbody = parentTransform.GetComponentInChildren<Rigidbody>();
                boxColliders = parentTransform.GetComponentsInChildren<BoxCollider>();
                isHeld = false;
                SetupClientRpc();
            }
        }*/
    }

    void SetUpItemClient(ulong clientId)
    {
        //tempPosition = transform.parent.position;
        if(IsServer) SetupClientRpc();
    }
    void SetUpItem()
    {
        if (IsServer)
        {
            //parentTransform = transform.parent.GetComponent<Transform>();
            //boxColliders = parentTransform.GetComponentsInChildren<BoxCollider>();
            isHeld = false;
            locked = false;
            SetupClientRpc();
        }
    }

    [ClientRpc]
    void SetupClientRpc()
    {
        if (!IsServer)
        {
            isHeld = false;
            locked = false;
            //transform.parent.position = tempPosition;
            //parentTransform = transform.parent;
        }
    }
    
    #endregion

    #region Networking

    private void OnValueChanged(Vector3 previousValue, Vector3 newValue)
    {
        if (previousValue != newValue)
        {
            transform.parent.position = newValue;
        }
    }
    
    //TODO: Implement some sort of cooldown so item can't be picked up again immediately
    //Otherwise you can "drop" it into another player
    //Or drop it on yourself and immediately pick it up
    [ClientRpc]
    public void GetDropPointClientRpc(Vector3 dropPoint)
    {
	    // TODO: CAM NOTE: Do you even need to do this on the client? Doesn't the item have a networked transform?
        //parentTransform.position = dropPoint;
    }

    //HACK: Delay to reduce the likelihood of item becoming reactive in it's old pos
    //then teleporting to new pos. Still happens occasionally unfortunately.
    [ClientRpc]
    public void GetDroppedClientRpc()
    {
	    // TODO: TO CHECK: HACK: I (Cam) added a GO var to the interface, so items know who picked them up. But clients can't really use that at all, so I'm just passing null. The implementor will need to check isServer to get the real value
        PutDown(null);
    }

    [ClientRpc]
    public void GetPickedUpClientRpc()
    {
	    // TODO: TO CHECK: HACK: I (Cam) added a GO var to the interface, so items know who picked them up. But clients can't really use that at all, so I'm just passing null. The implementor will need to check isServer to get the real value
        PickedUp(null);
    }
    
    #endregion
    
    #region IPickupable Interface

    public void DestroySelf()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public bool isHeld { get; set; }
    public bool locked { get; set; }
    public bool autoPickup { get; set; }

    public void PickedUp(GameObject interactor)
    {
        //parentTransform.gameObject.SetActive(false);
        isHeld = true;
        
        //replaced with hack
        //destroying objects and just respawning
        
        // if (!autoPickup && interactor.GetComponent<Interact>()!= null)
        // {
        //     interactor.GetComponent<Interact>().ParentItemObject(gameObject);
        // }
    }

    public void PutDown(GameObject interactor)
    {
        //parentTransform.gameObject.SetActive(true);
        isHeld = false;
        StartCoroutine(ItemLockCooldown());
    }
    
    #endregion
    
    //TODO: Idea
    //If picking up a second special item (pack pill thing), ITEM ITSELF should see that player already has this item
    //tell original item to int++ and then this (second) item deletes itself
    //then player spawns item based on int amount
    //if int amount == 0, then destroy placehold
    
    //HACK: Occasionally the object shows up for a frame in it's original position
    //before teleporting to it's correct position. WFS delay doesn't fix for some reason.
    private IEnumerator ItemLockCooldown()
    {
        locked = true;
        yield return new WaitForSeconds(1f);
        locked = false;
    }

    public void SetOnFire()
    {
	    
    }

    public void ChangeHeat(IHeatSource heatSource, float x)
    {
        throw new NotImplementedException();
    }
}
