using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oscar
{
    public class SpawnAI : MonoBehaviour
    {
        public GameObject AIEnemy;
        public void SpawnAIInTheMaze(Vector3 prefabPosition, GameObject AIParent)
        {
            //create spawn location with new perlin then 

            int spawnTheAI = Random.Range(1, 50);
            if (spawnTheAI == 1)
            {
                GameObject spawnedAI = Instantiate(AIEnemy, prefabPosition, Quaternion.identity);
                
                spawnedAI.transform.SetParent(AIParent.transform);
            }
        }
    }
}
