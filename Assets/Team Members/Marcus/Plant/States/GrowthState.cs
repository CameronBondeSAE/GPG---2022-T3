using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using NodeCanvas.Tasks.Actions;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marcus
{
    public class GrowthState : MonoBehaviour, IPickupable
    {
        public MonoBehaviour matureState;
        
        public GameObject seedling;
        public LayerMask spreadLayers;
        
        public int spreadLimit;
        public int spreadNumber;
        
        public float maxAge;
        public float age;

        private const bool AutoPickup = true;

        private float spreadTimer;
        private float spreadDistance;
        private Vector3 spreadDirection;

        public delegate void Growing();
        public event Growing GrowEvent;

        private NetworkBehaviour _stateManager;

        void OnEnable()
        {
	        NetworkManager nm = NetworkManager.Singleton;
	        if (nm.IsServer) StartCoroutine(Age());
	        if (nm.IsClient) GrowEvent?.Invoke();
        }

        IEnumerator Age()
        {
	        yield return new WaitUntil(AtMaxAge);
	        RandomiseTimer();
        }

        bool AtMaxAge()
        {
	        age += Time.deltaTime / 5;

	        while (age < maxAge)
	        {
		        return false;
	        }

	        return true;
        }
        
        void RandomiseTimer()
        {
	        spreadTimer = Random.Range(12f, 15f);
	        StartCoroutine(RandomTimer());
        }

        IEnumerator RandomTimer()
        {
	        yield return new WaitForSeconds(spreadTimer);
	        if (spreadNumber < spreadLimit)
	        {
		        spreadDistance = Random.Range(0.5f, 1.5f);
		        spreadDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;
		        Spread(spreadDistance, spreadDirection);
		        RandomiseTimer();
	        }
	        else
	        {
		        GetComponent<StateManager>().ChangeState(matureState);
	        }
        }

        void Spread(float distance, Vector3 direction)
        {
            Vector3 pos = transform.position + direction * distance;
        
            //grow new plants
            if (Physics.OverlapSphere(pos, maxAge/2, spreadLayers, QueryTriggerInteraction.Collide).Length == 0)
            {
                Instantiate(seedling, pos, Quaternion.identity);
            }
            spreadNumber++;
            RandomiseTimer();
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
            set {}
        }

        #endregion
    }   
}
