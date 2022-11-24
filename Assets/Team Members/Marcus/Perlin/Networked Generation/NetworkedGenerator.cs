using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using Oscar;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marcus
{
    public class NetworkedGenerator : MonoBehaviour, ILevelGenerate
    {
        private GameManager gm;
        
        public int amount;

        Vector3 aiPos;
        int aiLimit;

        Vector3 itemPos;
        float itemStep = 0.15f;

        Vector3 brickPos;
        Vector3 floorPos;
        float step = 0.1f;
        Vector2 startPoint = Vector2.one * 500;
    
        //Luke Said to use this rather than running everything through the original perlin
        private float[,] perlinGrid;
        private float[,] itemGrid;

        public GameObject playerHQ;
        public GameObject alienHQ;
        Vector3 hqPos;

        public GameObject pickup;
        public GameObject swarmer;
    
        public GameObject floorPrefab;
        public GameObject wallPrefab;
        public GameObject borderPrefab;

        private void Awake()
        {
            gm = GameManager.singleton;
            gm.LevelGenerator = this;
            
            perlinGrid = new float[amount, amount];
            itemGrid = new float[amount, amount];
        }

        private void OnDisable()
        {
            GameManager.singleton.LevelGenerator = null;
        }

        public void RandomiseValues()
        {
            step = Random.Range(0.08f, 0.12f);
            startPoint.x = Random.Range(0, 1000);
            startPoint.y = Random.Range(0, 1000);
        
            itemStep = Random.Range(0.05f, 0.15f);
        }

        public void SpawnPerlinClientRpc()
        {
            float floorPos = amount / 2f;

            GameObject floor = Instantiate(floorPrefab, new Vector3(floorPos, 0f, floorPos), Quaternion.identity);
            floor.transform.localScale = new Vector3(amount + 1, 0.1f, amount + 1);

            for (int x = 0; x < amount; x++)
            {
                for (int z = 0; z < amount; z++)
                {
                    brickPos.x = x;
                    brickPos.y = Mathf.PerlinNoise((x + startPoint.x) * step, (z + startPoint.y) * step);
                    brickPos.z = z;

                    if (brickPos.y < 0.5f)
                    {
                        perlinGrid[x, z] = 0;
                    }
                    else
                    {
                        Instantiate(wallPrefab, brickPos, Quaternion.identity);
                        perlinGrid[x, z] = 1;
                    }
                }
            }
        }

        public void SpawnBorderClientRpc()
        {
            GameObject curBorder;
        
            curBorder = Instantiate(borderPrefab, new Vector3(amount/2f, 2, -1), Quaternion.identity);
            curBorder.transform.localScale += new Vector3(amount, 0, 0);
        
            curBorder = Instantiate(borderPrefab, new Vector3(-1, 2, amount/2f), Quaternion.identity);
            curBorder.transform.localScale += new Vector3(0, 0, amount);
        
            curBorder = Instantiate(borderPrefab, new Vector3(amount/2f, 2, amount + 1), Quaternion.identity);
            curBorder.transform.localScale += new Vector3(amount, 0, 0);
        
            curBorder = Instantiate(borderPrefab, new Vector3(amount + 1, 2, amount/2f), Quaternion.identity);
            curBorder.transform.localScale += new Vector3(0, 0, amount);
        }

        public void SpawnAIClientRpc()
        {
            for (int x = 0; x < amount; x++)
            {
                for (int z = 0; z < amount; z++)
                {
                    aiPos = new Vector3(x, 0.1f, z);

                    if (itemGrid[x, z] == 0.5f)
                    {
                        if (aiLimit == 5)
                        {
                            Instantiate(swarmer, aiPos, Quaternion.identity);
                            aiLimit = 0;
                        }

                        aiLimit++;
                    }
                }
            }
        }

        public void SpawnItemsClientRpc()
        {
            for (int x = 0; x < amount; x++)
            {
                for (int z = 0; z < amount; z++)
                {
                    itemPos = new Vector3(x, 0.1f, z);

                    if (perlinGrid[x, z] == 0)
                    {
                        if (Mathf.PerlinNoise(x * itemStep, z * itemStep) >= 0.7f)
                        {
                            Instantiate(pickup, itemPos, Quaternion.identity);
                            itemGrid[x, z] = 1;
                        }
                        else if (Mathf.PerlinNoise(x * itemStep, z * itemStep) <= 0.2f)
                        {
                            itemGrid[x, z] = 0.5f;
                        }
                    }
                }
            }
        }

        public void SpawnExplosivesClientRpc()
        {
            print("I DO NOTHING YET");
        }

        public void SpawnBasesClientRpc()
        {
            for (int i = 0; i < 4; i++)
            {
                hqPos = new Vector3(Random.Range(0, amount), -1, Random.Range(0, amount));

                if (i == 0)
                {
                    Instantiate(playerHQ, hqPos, Quaternion.identity);
                }
                else
                {
                    Instantiate(alienHQ, hqPos, Quaternion.identity);
                }
            }
        }
    }
}
