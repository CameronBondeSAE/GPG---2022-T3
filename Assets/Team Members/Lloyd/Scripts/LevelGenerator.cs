using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using Oscar;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Lloyd
{
    public class LevelGenerator : MonoBehaviour, ILevelGenerate
    {
        public Luke.GameManager gameManager;
        
        [Header("Noise Settings")] [SerializeField]
        private int numCube;
        [SerializeField] private float cubeScale;

        //perlin noise stuff
        [SerializeField] private float zoomNoiseX;
        [SerializeField] private float zoomNoiseY;
        [SerializeField] private float cubeHeight;

        [SerializeField] private float wallsHeight;

        [Header("Terrain Object")] public GameObject cubePrefab;
        public GameObject wallPrefab;

        public GameObject _barrelObj;
        [SerializeField] private float numBarrels;
        
        [Header("Plant Prefab")] [SerializeField]
        private GameObject itemPrefab;

        [SerializeField] private List<Vector3> itemVector3List;
        private Vector3 itemPos;
        [SerializeField] private float itemNoiseX;
        [SerializeField] private float itemNoiseY;

        private Vector3 centrePos;

        private Vector3 humanPos;

        private float distanceFromCentre;

        [SerializeField] private float minDist;

        [SerializeField] private int numItems;

        [Header("HQ")] [SerializeField] private GameObject HumanHQ;

        [SerializeField] private GameObject playerPrefab;

        private HQ hqscript;

        [SerializeField] private float destroyRadius;

        [SerializeField] private int numHumanHQ;

        [SerializeField] private GameObject alienSmart;
        [SerializeField] private GameObject alienDumb;
        [SerializeField] private GameObject AlienHQ;
        [SerializeField] private int numAlienHQ;
        public List<Vector3> alienHQVector3List = new List<Vector3>();

        private Vector3 alienPos;
        private Vector3 prevAlienPos;

        private GameObject terrainParent;
        private GameObject itemParent;
        private GameObject environmentParent;
        private GameObject HQParent;
        private GameObject PlayerParent;
        private GameObject AlienParent;

        //cube stuff
        private Vector3 cubePos;
        private Renderer cubeRend;
        private Rigidbody rb;
        private GameObject cubeSpawned;
        private bool isHighest;

        public void SpawnPerlin()
        {
            GenerateTerrain();
        }

        public void SpawnBorder()
        {
            PlaceGround();
            PlaceWalls();
        }

        public void SpawnAI()
        {
            
        }

        public void SpawnItems()
        {
             SpawnPlants();
        }

        public void SpawnExplosives()
        {
            
        }

        public void SpawnBases()
        {
            PlaceHQ();

            SpawnAlienHQ();
        }

        public void GenerateTerrain()
        {
            Destroy(terrainParent);
            Destroy(itemParent);
            Destroy(environmentParent);
            Destroy(HQParent);
            Destroy(PlayerParent);
            Destroy(AlienParent);

            terrainParent = new GameObject("terrainParent");
            itemParent = new GameObject("itemParent");
            environmentParent = new GameObject("environmentParent");
            HQParent = new GameObject("HQParent");
            PlayerParent = new GameObject("PlayerParent");
            AlienParent = new GameObject("AlienParent");
            SpawnTerrain();
        }

        void SpawnTerrain()
        {
            itemVector3List = new List<Vector3>(numItems);

            alienHQVector3List = new List<Vector3>(numAlienHQ);

            for (float x = 0; x < numCube; x = x + cubeScale)
            {
                for (float z = 0; z < numCube; z = z + cubeScale)
                {
                    centrePos = new Vector3(cubePos.x / 2, cubePos.y / 2, cubePos.z / 2);

                    cubePos.x = x;
                    float perlinNoise = Mathf.PerlinNoise(x * zoomNoiseX, z * zoomNoiseY);
                    cubePos.y = perlinNoise * cubeHeight;
                    cubePos.z = z;

                    if (perlinNoise < 0.1f && x > 33 && z > 33)
                    {
                        SpawnAlienHQPos();
                    }

                    if (perlinNoise < 0.2)
                    {
                        SpawnItemPos();
                    } 

                    if (perlinNoise > 0.5)
                    {
                        GameObject cube = Instantiate(wallPrefab, cubePos, Quaternion.identity) as GameObject;
                        cube.transform.SetParent(terrainParent.transform);

                            //cube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);

                            cubeRend = cube.GetComponent<Renderer>();
                        cubeRend.material.color = Color.gray;

                        if (perlinNoise > .9f)
                        {
                            cubeRend = cube.GetComponent<Renderer>();

                            cubeRend.material.color = Color.red;
                        }

                        //changes the size of the bottom cubes to stretch towards the ground (ostensibly prevents running underneath & fills gaps)
                        //note Mathf.Abs to fix box collider problem
                        Resize(cubeScale, new Vector3(0f, Mathf.Abs(-1*cubeHeight), 0f));

                        void Resize(float amount, Vector3 direction)
                        {
                            //cube.transform.position += direction * amount / 2;
                            cube.transform.localScale += direction * amount;
                        }
                    }
                }
            }
            gameManager.SpawnPerlinFinished();
        }

        //spawns item transforms in List itemVector3List
        //items are then spawned from Spawn Items
        void SpawnItemPos()
        {
            itemPos = new Vector3(cubePos.x, 1, cubePos.z);

            float itemNoise = Mathf.PerlinNoise(itemNoiseX, itemNoiseY);

            if ((itemNoise > .35f) && (itemNoise < .65f) && numItems > 0)
            {
                Vector3 tempItemPos;
                tempItemPos = itemPos;
                itemVector3List.Add(tempItemPos);
            }
        }

        void SpawnPlants()
        {
            foreach (Vector3 tempItemPos in itemVector3List)
            {
                GameObject item =
                    Instantiate(itemPrefab, tempItemPos, Quaternion.identity) as GameObject;
                item.transform.SetParent(itemParent.transform);
                cubeRend = item.GetComponentInChildren<Renderer>();
                cubeRend.material.color = Color.green;
            }
            gameManager.SpawnItemsFinished();
        }

        void PlaceWalls()
        {
            GameObject wall01 = Instantiate(cubePrefab, new Vector3(cubePos.x / 2, cubePos.y, cubePos.z + 0),
                Quaternion.identity);
            wall01.name = "Wall";
            wall01.transform.localScale = new Vector3(cubePos.x, wallsHeight, 1);
            wall01.transform.SetParent(environmentParent.transform);
            cubeRend = wall01.GetComponent<Renderer>();
            cubeRend.material.color = Color.blue;

            GameObject wall02 = Instantiate(cubePrefab, new Vector3(cubePos.x + 0, cubePos.y, cubePos.z / 2),
                Quaternion.identity);
            wall02.name = "Wall";
            wall02.transform.localScale = new Vector3(1, wallsHeight, cubePos.z);
            cubeRend = wall02.GetComponent<Renderer>();
            wall02.transform.SetParent(environmentParent.transform);
            cubeRend.material.color = Color.blue;

            GameObject wall03 =
                Instantiate(cubePrefab, new Vector3(cubePos.x / 2, cubePos.y, 0), Quaternion.identity);
            wall03.name = "Wall";
            wall03.transform.localScale = new Vector3(cubePos.x, wallsHeight, 1);
            wall03.transform.SetParent(environmentParent.transform);
            cubeRend = wall03.GetComponent<Renderer>();
            cubeRend.material.color = Color.blue;

            GameObject wall04 =
                Instantiate(cubePrefab, new Vector3(0, cubePos.y, cubePos.z / 2), Quaternion.identity);
            wall04.name = "Wall";
            wall04.transform.localScale = new Vector3(1, wallsHeight, cubePos.z);
            wall04.transform.SetParent(environmentParent.transform);
            cubeRend = wall04.GetComponent<Renderer>();
            cubeRend.material.color = Color.blue;
        }

        void PlaceGround()
        {
            GameObject ground = Instantiate(cubePrefab, centrePos, Quaternion.identity);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(cubePos.x, Mathf.Abs(cubePos.y-cubeScale), cubePos.z);
            cubeRend = ground.GetComponent<Renderer>();
            cubeRend.material.color = Color.black;

            ground.transform.position = new Vector3(cubePos.x/2, 0, cubePos.z/2 );

            ground.transform.SetParent(environmentParent.transform);
            gameManager.SpawnBorderFinished();
        }

        public void RandomiseValues()
        {
            cubeHeight = Random.Range(2f, 5f);
            zoomNoiseX = Random.Range(.07f, .2f);
            zoomNoiseY = Random.Range(.07f, .2f);

            itemNoiseX = Random.Range(.0f, .10f);
            itemNoiseY = Random.Range(.0f, .10f);
        }

        ///////////////////////////////////////////////////////////////////
        ///
        /// how to make HQ spawn locations more interesting (read perlin)
        ///
        private void PlaceHQ()
        {
            GameObject HumanHQprefab = Instantiate(HumanHQ,
                new Vector3(centrePos.x, (centrePos.y + cubeScale) / 2, centrePos.z), Quaternion.identity);
            HumanHQprefab.transform.SetParent(HQParent.transform);
            
            hqscript = HumanHQprefab.GetComponentInChildren<HQ>();
            hqscript.DestroyLand(destroyRadius);

            /*GameObject player = Instantiate(playerPrefab, HumanHQprefab.transform.position, Quaternion.identity);
            player.transform.SetParent(PlayerParent.transform);*/
        }

        private void SpawnAlienHQPos()
        {
            alienPos = new Vector3(cubePos.x, cubePos.y + (cubeScale / 2), cubePos.z);

            float tempAlienDist = Vector3.Distance(alienPos, prevAlienPos);

            if (tempAlienDist > minDist)
            {
                float tempAlienDistTwo = Vector3.Distance(alienPos, centrePos);
                if (tempAlienDistTwo > minDist)
                {
                    {
                        if (alienHQVector3List.Count < numAlienHQ)
                        {
                            Vector3 tempAlienPos;
                            tempAlienPos = alienPos;
                            alienHQVector3List.Add(tempAlienPos);
                            prevAlienPos = tempAlienPos;
                        }
                    }
                }
            }
        }

        private void SpawnAlienHQ()
        {
            foreach (Vector3 tempAlienPos in alienHQVector3List)
            {
                GameObject AlienHQprefab = Instantiate(AlienHQ, tempAlienPos, Quaternion.identity);
                AlienHQprefab.transform.SetParent(HQParent.transform);
                
                GameObject alienLeader = Instantiate(alienSmart, tempAlienPos, Quaternion.identity);
                GameObject alienPeon = Instantiate(alienDumb, tempAlienPos, Quaternion.identity);

                alienLeader.transform.SetParent(AlienParent.transform);
                alienPeon.transform.SetParent(alienLeader.transform);

                hqscript = AlienHQprefab.GetComponentInChildren<HQ>();
                hqscript.DestroyLand(destroyRadius);
                
            }
            gameManager.SpawnBasesFinished();
        }
    }
}