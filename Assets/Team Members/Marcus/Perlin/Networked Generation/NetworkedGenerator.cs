using System.Collections;
using System.Collections.Generic;
using Oscar;
using UnityEngine;

namespace Marcus
{
    public class NetworkedGenerator : MonoBehaviour, ILevelGenerate
    {
        public int amount;

        Vector3 aiPos;
        int aiLimit;

        Vector3 itemPos;
        float itemZoom;

        Vector3 brickPos;
        Vector3 floorPos;
        float zoom;
        Vector2 randomOffset;
    
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
        
        // Start is called before the first frame update
        void Start()
        {
            perlinGrid = new float[amount, amount];
            itemGrid = new float[amount, amount];
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void SpawnPerlin()
        {
            float floorPos = amount / 2f;

            GameObject floor = Instantiate(floorPrefab, new Vector3(floorPos, 0f, floorPos), Quaternion.identity);
            floor.transform.localScale = new Vector3(amount + 1, 0.1f, amount + 1);

            for (int x = 0; x < amount; x++)
            {
                for (int z = 0; z < amount; z++)
                {
                    brickPos.x = x;
                    brickPos.y = Mathf.PerlinNoise((x + 500/*startPoint.x*/) * 0.1f/*step*/, (z + 500/*startPoint.y*/) * 0.1f/*step*/);
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

        public void SpawnBorder()
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

        public void SpawnAI()
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

        public void SpawnItems()
        {
            for (int x = 0; x < amount; x++)
            {
                for (int z = 0; z < amount; z++)
                {
                    itemPos = new Vector3(x, 0.1f, z);

                    if (perlinGrid[x, z] == 0)
                    {
                        if (Mathf.PerlinNoise(x * 0.15f/*itemStep*/, z * 0.15f/*itemStep*/) >= 0.7f)
                        {
                            Instantiate(pickup, itemPos, Quaternion.identity);
                            itemGrid[x, z] = 1;
                        }
                        else if (Mathf.PerlinNoise(x * 0.15f/*itemStep*/, z * 0.15f/*itemStep*/) <= 0.2f)
                        {
                            itemGrid[x, z] = 0.5f;
                        }
                    }
                }
            }
        }

        public void SpawnExplosives()
        {
            print("I DO NOTHING YET");
        }

        public void SpawnBases()
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
