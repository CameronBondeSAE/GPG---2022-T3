using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oscar
{
    public class SpawnAI : MonoBehaviour
    {
        public GameObject AIEnemy;

        private float AIInt = .2f;
        public int AIAmount = 0;
        public int waitAmount = 0;
        
        public void SpawnAIInTheMaze(Vector3 prefabPosition, GameObject AIParent, float perlinValue)
        {
            float AIPerlin = Mathf.PerlinNoise((AIInt), (AIInt));
            //create spawn location with new perlin then 

            if (AIPerlin > .5f && AIAmount <= 20 && waitAmount >= 100)
            {
                GameObject spawnedAI = Instantiate(AIEnemy, prefabPosition, Quaternion.identity);
                
                spawnedAI.transform.SetParent(AIParent.transform);
                AIAmount++;
                waitAmount = 0;
            }
            else
            {
                waitAmount++;
            }
        }
    }
}
