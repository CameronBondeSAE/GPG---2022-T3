using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class MatureState : MonoBehaviour, IFlammable
    {
        public MonoBehaviour dyingState;
        
        public GameObject manEater;
        public LayerMask evolutionLayer;
    
        private float deathTimer;
        
        // Start is called before the first frame update
        void Start()
        {
            RandomiseTimer();
        }
        
        void RandomiseTimer()
        {
            deathTimer = Random.Range(4f, 7f);
        }

        // Update is called once per frame
        void Update()
        {
            deathTimer -= Time.deltaTime;

            if (deathTimer <= 0)
            {
                int rEvoChance = Random.Range(0, 5);
                
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

        public void SetOnFire()
        {
            print("MYAH!!! am old ded ;-;");
        }

        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            
        }

        #endregion
    }
}
