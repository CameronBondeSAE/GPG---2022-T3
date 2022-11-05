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

        public MapGenerator mapGenerator;
        
        private void OnEnable()
        {
            // mapGenerator.SpawnCubes += SpawningTheExplosives(Vector3 prefabPosition, GameObject explosiveParent, float perlinValue);
        }

        public void SpawningTheExplosives(Vector3 prefabPosition, GameObject explosiveParent, float perlinValue)
        {
            if (perlinValue < .5)
            {
                
                //use new perlin to create clusters of barrels
                int SpawmTheBarrels = Random.Range(1, 50);
                if (SpawmTheBarrels == 1)
                {
                    GameObject spawnedItem = Instantiate(explosive[Random.Range(0,2)], prefabPosition, quaternion.identity);
                    
                    spawnedItem.transform.SetParent(explosiveParent.transform);
                }
            }
            
        }
    }

}
