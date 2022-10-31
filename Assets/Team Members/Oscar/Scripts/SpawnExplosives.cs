using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Oscar
{
    public class SpawnExplosives : MonoBehaviour
    {
        public GameObject explosive;
        
        public void SpawningTheExplosives(Vector3 prefabPosition, GameObject explosiveParent)
        {
            
            //use new perlin to create clusters of barrels
            int SpawmTheBarrels = Random.Range(1, 50);
            if (SpawmTheBarrels == 1)
            {
                GameObject spawnedItem = Instantiate(explosive, prefabPosition, quaternion.identity);
                
                spawnedItem.transform.SetParent(explosiveParent.transform);
                spawnedItem.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

}
