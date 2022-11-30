using System;
using System.Collections;
using System.Collections.Generic;
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
        
        public delegate void Matured(float duration);
        public event Matured MatureEvent ;
        
        void OnEnable()
        {
            deathTimer = Random.Range(4f, 7f);
            rEvoChance = Random.Range(0, 10);
            
            MatureEvent?.Invoke(deathTimer);
        }

        // Update is called once per frame
        void Update()
        {
            deathTimer -= Time.deltaTime;

            if (deathTimer <= 0)
            {
                if (Physics.OverlapSphere(transform.position, 1, evolutionLayer, QueryTriggerInteraction.Collide).Length >=5 && rEvoChance == 1)
                { 
                    //Destroy and spawn manEater prefab
                    Instantiate(manEater, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                else
                {
                    //Change to dying State
                    GetComponent<StateManager>().ChangeState(dyingState);
                }
            }
        }

        #region Interface Functions

        public void PickedUp(GameObject interactor)
        {
            
        }

        public void PutDown(GameObject interactor)
        {
            
        }

        public void DestroySelf()
        {
            
        }

        public bool isHeld { get; set; }
        public bool locked { get; set; }

        public bool autoPickup
        {
            get
            {
                return true;
            }
            set
            {
                
            }
        }

        #endregion
    }
}
