using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PerlinScript : MonoBehaviour
{
    [Header("Noise Settings")] [SerializeField]
        private int numCube;
        
        [SerializeField] private float zoomX;
        [SerializeField] private float zoomY;
        [SerializeField] private float scale;

        [SerializeField] private float wallsHeight;
    
    [Header("Terrain Object")] public GameObject cubePrefab;

    [Header("Item Prefab")] [SerializeField]
    private GameObject itemPrefab;

    private Vector3 itemPos;

    [SerializeField] private int numItems;

    

    

    [Header("Parent Objs")] [SerializeField]
    public GameObject terrainParent;
    public GameObject itemParent;
    public GameObject environmentParent;
    public GameObject HQParent;
    

    private Vector3 cubePos;
    private Renderer cubeRend;
    private GameObject cubeSpawned;
    private bool isHighest;

    private void Start()
    {
        GenerateTerrain();
        
    } 
    
    public void GenerateTerrain()
    {
        EventManager.TerrainClearFunction();
        SpawnTerrain();
    }

    void SpawnTerrain()
    {
        for (int x = 0; x < numCube; x++)
        {
            for (int z = 0; z < numCube; z++)
            {
                cubePos.x = x;
                float perlinNoise = Mathf.PerlinNoise(x * zoomX, z * zoomY);
                cubePos.y = perlinNoise * scale;
                cubePos.z = z;

                if (perlinNoise < 0.1f)
                {
                    SpawnAlienHQ();
                }

                if (perlinNoise < 0.2)
                {
                    SpawnItems();
                }

                if (perlinNoise > 0.4)
                {
                    GameObject cube = Instantiate(cubePrefab, cubePos, Quaternion.identity) as GameObject;
                    cube.transform.SetParent(terrainParent.transform);
                    if (perlinNoise > .6f)
                    {
                        cubeRend = cube.GetComponent<Renderer>();
                        cubeRend.material.color = Color.red;
                    }

                    if (perlinNoise < .5f)
                    {
                        cubeRend = cube.GetComponent<Renderer>();
                        cubeRend.material.color = Color.black;
                        
                        
                    }
                    
                    //changes the size of the bottom cubes to stretch towards the ground (ostensibly prevents running underneath & fills gaps)
                    //note Mathf.Abs to fix box collider problem
                    Resize(3f, new Vector3(0f, Mathf.Abs(-1), 0f));
                    void Resize(float amount, Vector3 direction)
                    {
                        cube.transform.position += direction * amount / 2;
                        cube.transform.localScale += direction * amount;
                    }
                }
            }
            
        }

        PlaceGround();
        PlaceWalls();

        PlaceHQ();
    }
    
    

    void SpawnItems()
    {
        //how to add as a for loop? 
        
        itemPos = new Vector3(cubePos.x, cubePos.y + (1*scale), cubePos.z);
        float randomX = (Random.Range(1, 10f));
        float randomY = (Random.Range(1, 10f));
        
        float itemNoise = Mathf.PerlinNoise(randomX, randomY);
        
            if ((itemNoise > .35f) && (itemNoise < .84f) && numItems > 0)
            {
                GameObject item =
                    Instantiate(itemPrefab, itemPos, Quaternion.identity) as GameObject;
                item.transform.SetParent(itemParent.transform);
                cubeRend = item.GetComponent<Renderer>();
                cubeRend.material.color = Color.yellow;
                numItems--;
            }
    }

    void PlaceWalls()
    {
        GameObject wall01 = Instantiate(cubePrefab, new Vector3(cubePos.x/2, cubePos.y, cubePos.z+.5f), Quaternion.identity);
        wall01.name = "Wall";
        wall01.transform.localScale = new Vector3(cubePos.x, wallsHeight, 1);
       
        cubeRend = wall01.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
        
        GameObject wall02 = Instantiate(cubePrefab, new Vector3(cubePos.x+.5f, cubePos.y, cubePos.z/2), Quaternion.identity);
        wall02.name = "Wall";
        wall02.transform.localScale = new Vector3(1, wallsHeight, cubePos.z);
        cubeRend = wall02.GetComponent<Renderer>();
        wall02.transform.SetParent(environmentParent.transform);
        cubeRend.material.color = Color.blue;
        
        GameObject wall03 = Instantiate(cubePrefab, new Vector3(cubePos.x/2, cubePos.y, -.5f), Quaternion.identity);
        wall03.name = "Wall";
        wall03.transform.localScale = new Vector3(cubePos.x, wallsHeight, 1);
        wall03.transform.SetParent(environmentParent.transform);
        cubeRend = wall03.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
        
        GameObject wall04 = Instantiate(cubePrefab, new Vector3(-.5f, cubePos.y, cubePos.z/2), Quaternion.identity);
        wall04.name = "Wall";
        wall04.transform.localScale = new Vector3(1, wallsHeight, cubePos.z);
        wall04.transform.SetParent(environmentParent.transform);
        cubeRend = wall04.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
    }

    void PlaceGround()
    {
        GameObject ground = Instantiate(cubePrefab, new Vector3(cubePos.x / 2, cubePos.y/2, cubePos.z / 2), Quaternion.identity);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(cubePos.x, Mathf.Abs( -cubePos.y), cubePos.z);
        cubeRend = ground.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
    }

    public void RandomiseValues()
    {
        scale = Random.Range(5f, 8f);
        zoomX = Random.Range(.07f, .2f);
        zoomY = Random.Range(.07f, .2f);
    }
    
    ///////////////////////////////////////////////////////////////////
    ///
    /// how to make HQ spawn locations more interesting (read perlin)
    ///
    [SerializeField] private GameObject HumanHQ;
    [SerializeField] private int numHumanHQ;
    
    [SerializeField] private GameObject AlienHQ;
    [SerializeField] private int numAlienHQ;

    private void PlaceHQ()
    {
        for (int x = 0; x < numHumanHQ; x++)
        {
            GameObject HumanHQprefab = Instantiate(HumanHQ,
                new Vector3(cubePos.x / 2, (cubePos.y + scale) / 2, cubePos.z / 2), Quaternion.identity);
            HumanHQprefab.transform.SetParent(HQParent.transform);
        }
    }

    private void SpawnAlienHQ()
    {
        int random;
        random = Random.Range(1, 100);

        Vector3 HumanHQPos = HumanHQ.transform.position;
        
        float dist = Vector3.Distance(HumanHQ.transform.position, this.transform.position);

        if ((numAlienHQ > 0) && (random > 95) )
        {
            GameObject AlienHQprefab = Instantiate(AlienHQ,
                new Vector3(cubePos.x, (cubePos.y + scale) / 2, cubePos.z),
                Quaternion.identity);
            AlienHQprefab.transform.SetParent(HQParent.transform);
            numAlienHQ--;
        }
    }

}