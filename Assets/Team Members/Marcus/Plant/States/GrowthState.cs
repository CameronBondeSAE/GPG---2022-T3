using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class GrowthState : MonoBehaviour
    {
        public GameObject seedling;
        public LayerMask spreadLayers;
        
        public int spreadLimit;
        private int spreadNumber;
        
        private float maxSize;
        
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
            if (transform.localScale.magnitude >= maxSize)
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
                    //Change to Mature State
                }
            }
        }
        
        void Spread(float distance, Vector3 direction)
        {
            Vector3 pos = transform.position + direction * distance;
        
            //grow new plants
            if (Physics.OverlapSphere(pos, maxSize/2, spreadLayers, QueryTriggerInteraction.Collide).Length == 0)
            {
                Instantiate(seedling, pos, Quaternion.identity);
            }
            spreadNumber++;
            RandomiseTimer();
        }
    }   
}
