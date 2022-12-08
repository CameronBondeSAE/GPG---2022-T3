using System.Collections;
using System.Collections.Generic;
using Luke;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Seed : MonoBehaviour
{
    public GameObject plant;

    void OnEnable()
    {
	    GameManager.singleton.NetworkInstantiate(plant, transform.position, Quaternion.identity);
	    Destroy(gameObject);
    }
}
