using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Luke;
using Unity.Netcode;
using UnityEngine;

namespace Oscar
{
    public class SpawnAI : NetworkBehaviour
    {
        public GameObject AIEnemy;

        private float AIInt = .2f;
        public int AIAmount = 0;
        public int waitAmount = 0;
        public GameObject alienBase;

        public GameObject AlienBase()
        {
            GameObject spawnPointObject = null;
            foreach (HQ hq in FindObjectsOfType<HQ>())
            {
                if (hq.type == HQ.HQType.Aliens)
                {
                    spawnPointObject = hq.GetComponentInChildren<SpawnPoint>().gameObject;
                }
            }

            return spawnPointObject;
        }
        public void SpawnAIInTheMaze(List<Vector3> prefabPosition, GameObject AIParent, float perlinValue)
        {
            float AIPerlin = Mathf.PerlinNoise((AIInt), (AIInt));
            //create spawn location with new perlin then 
            if (GameManager.singleton.amountOfAIInGame < 100)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (AIPerlin > .5f && waitAmount <= 100)
                    {
                        GameManager.singleton.NetworkInstantiate(AIEnemy, AlienBase().GetComponent<SpawnPoint>().transform.position, Quaternion.identity);

                        //spawnedAI.transform.SetParent(AIParent.transform);
                        AIAmount++;
                        GameManager.singleton.amountOfAIInGame++;
                        waitAmount = 0;

                    }
                    else
                    {
                        waitAmount++;
                    }
                }

            }

        }
    }
}
