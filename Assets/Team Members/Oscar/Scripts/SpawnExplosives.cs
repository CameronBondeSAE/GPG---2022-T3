using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Oscar
{
    public class SpawnExplosives : MonoBehaviour
    {
        public GameObject[] explosive;

        private float boomInt = .4f;
        
        private int boomAmount = 0;
        private int waitAmount = 0;

        public void SpawningTheExplosives(Vector3 prefabPosition, GameObject explosiveParent, float perlinValue)
        {            
            //create spawn location with new perlin then 
            float explosivesPerlin = Mathf.PerlinNoise((boomInt), (boomInt));

            //use new perlin to spawn explosives
            if (explosivesPerlin < .4 && boomAmount <= 50 && waitAmount >= 110)
            {
                GameObject spawnedItem = Instantiate(explosive[Random.Range(0,2)], prefabPosition, quaternion.identity);
                
                spawnedItem.transform.SetParent(explosiveParent.transform);

                boomAmount++;
                waitAmount = 0;
            }
            else
            {
                waitAmount++;
            }
            
        }
    }

}
