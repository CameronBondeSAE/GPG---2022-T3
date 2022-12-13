using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marcus
{
    public class MatureState : MonoBehaviour, IPickupable
    {
        public MonoBehaviour dyingState;
        
        public GameObject manEater;
        public LayerMask evolutionLayer;
    
        public float deathTimer;
        private int rEvoChance;
        
        private const bool AutoPickup = true;

        public delegate void Matured(float duration);
        public event Matured MatureEvent;
        
        void OnEnable()
        {
	        NetworkManager nm = NetworkManager.Singleton;
	        if (nm.IsServer)
	        {
		        deathTimer = Random.Range(4f, 7f);
		        rEvoChance = Random.Range(0, 10);
		        StartCoroutine(DeathTimer());
	        }
	        if(nm.IsClient) MatureEvent?.Invoke(deathTimer);
        }
        
        private void Start()
        {
	        // in here to allow enabling/disabling
        }

        IEnumerator DeathTimer()
        {
	        yield return new WaitForSeconds(deathTimer);
	        if (Physics.OverlapSphere(transform.position, 1, evolutionLayer, QueryTriggerInteraction.Collide).Length >=7 && rEvoChance == 1)
	        { 
		        //Destroy and spawn manEater prefab
		        GameManager.singleton.NetworkInstantiate(manEater, transform.position, Quaternion.identity);
		        Destroy(gameObject);
	        }
	        else
	        {
		        //Change to dying State
		        GetComponent<StateManager>().ChangeState(dyingState);
	        }
        }

        #region Interface Functions

        public void PickedUp(GameObject interactor, ulong networkObjectId)
        {
            
        }

        public void PutDown(GameObject interactor, ulong networkObjectId)
        {
            
        }

        public void DestroySelf()
        {
	        Destroy(gameObject);
        }

        public bool isHeld { get; set; }
        public bool locked { get; set; }

        public bool autoPickup
        {
	        get => AutoPickup;
            set { }
        }

        #endregion
    }
}
