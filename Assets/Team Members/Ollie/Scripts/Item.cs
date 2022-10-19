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

public class Item : NetworkBehaviour, IGoalItem
{
    public NetworkManager networkManager;
    public NetworkVariable<Vector3> networkPosition;
    public Transform parentTransform;
    private Vector3 tempPosition;
    private Transform tempParentTransform;
    //public BoxCollider boxCollider;
    public BoxCollider[] boxColliders;
    public bool isHeld { get; set; }
    public bool locked { get; set; }
    public ItemType itemType;

    private void Awake()
    {
        networkManager = NetworkManager.Singleton;
        networkManager.OnServerStarted += SetUpItem;
        networkManager.OnClientConnectedCallback += SetUpItemClient;
        networkPosition.OnValueChanged += OnValueChanged;
    }

    private void Start()
    {
        SetUpItem();
    }

    private void OnValueChanged(Vector3 previousValue, Vector3 newValue)
    {
        if (previousValue != newValue)
        {
            transform.parent.position = newValue;
        }
    }

    private void OnDisable()
    {
        networkManager.OnServerStarted -= SetUpItem;
        networkManager.OnClientConnectedCallback -= SetUpItemClient;
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
        parentTransform.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void GetPickedUpClientRpc()
    {
        parentTransform.gameObject.SetActive(false);
    }
}
