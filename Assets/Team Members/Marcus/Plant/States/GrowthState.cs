using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private float spreadTimer;
        private float spreadDistance;
        private Vector3 spreadDirection;
        
        // Start is called before the first frame update
        void Start()
        {
            RandomiseTimer();
        }

        void RandomiseTimer()
        {
            spreadTimer = Random.Range(2f, 5f);
        }
        
        // Update is called once per frame
        void Update()
        {
            age = Mathf.MoveTowards(age, maxAge, 0.002f);

            if (age >= maxAge)
            {
                spreadTimer -= Time.deltaTime;

                if (spreadTimer <= 0 && spreadNumber < spreadLimit)
                {
                    spreadDistance = Random.Range(1f, 3f);
                    spreadDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;

                    Spread(spreadDistance, spreadDirection);
                }
                else if (spreadNumber == spreadLimit)
                {
                    //Change to Mature state
                    GetComponent<StateManager>().ChangeState(matureState);
                }
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

        public void PickedUp(GameObject interactor)
        {
            
        }

        public void PutDown(GameObject interactor)
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
