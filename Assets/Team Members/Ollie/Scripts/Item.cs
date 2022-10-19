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

public class Item : NetworkBehaviour, IGoalItem, IPickupable
{
    public NetworkManager networkManager;
    public NetworkVariable<Vector3> networkPosition;
    public Transform parentTransform;
    private Vector3 tempPosition;
    private Transform tempParentTransform;
    public BoxCollider[] boxColliders;
    
    public ItemType itemType;

    #region SetUps

    private void Awake()
    {
        networkManager = NetworkManager.Singleton;
        networkManager.OnServerStarted += SetUpItem;
        networkManager.OnClientConnectedCallback += SetUpItemClient;
        networkPosition.OnValueChanged += OnValueChanged;
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
        tempPosition = transform.parent.position;
        if(IsServer) SetupClientRpc();
    }
    void SetUpItem()
    {
        if (IsServer)
        {
            parentTransform = transform.parent.GetComponent<Transform>();
            boxColliders = parentTransform.GetComponentsInChildren<BoxCollider>();
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
            transform.parent.position = tempPosition;
            parentTransform = transform.parent.GetComponent<Transform>();
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
        parentTransform.position = dropPoint;
    }

    //HACK: Delay to reduce the likelihood of item becoming reactive in it's old pos
    //then teleporting to new pos. Still happens occasionally unfortunately.
    [ClientRpc]
    public void GetDroppedClientRpc()
    {
        PutDown();
    }

    [ClientRpc]
    public void GetPickedUpClientRpc()
    {
        PickedUp();
    }
    
    #endregion
    
    #region IPickupable Interface
    
    public bool isHeld { get; set; }
    public bool locked { get; set; }
    public void PickedUp()
    {
        parentTransform.gameObject.SetActive(false);
    }

    public void PutDown()
    {
        parentTransform.gameObject.SetActive(true);
        StartCoroutine(ItemLockCooldown());
    }
    
    #endregion
    
    //HACK: Occasionally the object shows up for a frame in it's original position
    //before teleporting to it's correct position. WFS delay doesn't fix for some reason.
    private IEnumerator ItemLockCooldown()
    {
        locked = true;
        yield return new WaitForSeconds(1f);
        locked = false;
    }
}
