using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PerlinScript : MonoBehaviour
{
    [Header("Noise Settings")] [SerializeField]
    private int numCube;
    //perlin noise stuff
    [SerializeField] private float zoomNoiseX;
    [SerializeField] private float zoomNoiseY;
    [SerializeField] private float scale;
    
    [SerializeField] private float wallsHeight;

    [Header("Terrain Object")] public GameObject cubePrefab;

    [Header("Item Prefab")] [SerializeField]
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
    
    [Header("HQ")] [SerializeField] 
    private GameObject HumanHQ;
                                          
    [SerializeField] private int numHumanHQ;
                                          
    [SerializeField] private GameObject AlienHQ;
    [SerializeField] private int numAlienHQ;
    public List<Vector3> alienHQVector3List = new List<Vector3>();

    private Vector3 alienPos;
    private Vector3 prevAlienPos;
    
    [Header("Parent Objects")] [SerializeField]
    public GameObject terrainParent;

    public GameObject itemParent;
    public GameObject environmentParent;
    public GameObject HQParent;
    
    //cube stuff
    private Vector3 cubePos;
    private Renderer cubeRend;
    private Rigidbody rb;
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
        itemVector3List = new List<Vector3>(numItems);

        alienHQVector3List = new List<Vector3>(numAlienHQ);
        
        for (int x = 0; x < numCube; x++)
        {
            for (int z = 0; z < numCube; z++)
            {
                centrePos = new Vector3(cubePos.x / 2, cubePos.y / 2, cubePos.z / 2);

                cubePos.x = x;
                float perlinNoise = Mathf.PerlinNoise(x * zoomNoiseX, z * zoomNoiseY);
                cubePos.y = perlinNoise * scale;
                cubePos.z = z;

                if (perlinNoise < 0.1f && x > 20 && z > 20)
                {
                    SpawnAlienHQPos();
                }

                if (perlinNoise < 0.2)
                {
                    SpawnItemPos();
                }

                if (perlinNoise > 0.4)
                {
                    GameObject cube = Instantiate(cubePrefab, cubePos, Quaternion.identity) as GameObject;
                    cube.transform.SetParent(terrainParent.transform);
                    
                    cubeRend = cube.GetComponent<Renderer>();
                    cubeRend.material.color = Color.black;

                    if (perlinNoise < .5f)
                    {
                        cubeRend = cube.GetComponent<Renderer>();
                        cubeRend.material.color = Color.white;
                    }

                    if (perlinNoise > .6f)
                    {
                        cubeRend = cube.GetComponent<Renderer>();
                        
                        cubeRend.material.color = Color.red;
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

        PlaceHQ();
        
        SpawnAlienHQ();

        PlaceGround();
        PlaceWalls();
        SpawnItems();
        
    }

    //spawns item transforms in List itemVector3List
    //items are then spawned from Spawn Items
    void SpawnItemPos()
    {
        itemPos = new Vector3(cubePos.x, cubePos.y + (scale / 2), cubePos.z);

        float itemNoise = Mathf.PerlinNoise(itemNoiseX, itemNoiseY);

        if ((itemNoise > .35f) && (itemNoise < .65f) && numItems > 0)
        {
            Vector3 tempItemPos;
            tempItemPos = itemPos;
            itemVector3List.Add(tempItemPos);
        }
    }
    
    void SpawnItems()
    {
        foreach (Vector3 tempItemPos in itemVector3List)
        {
            GameObject item =
                Instantiate(itemPrefab, tempItemPos, Quaternion.identity) as GameObject;
            item.transform.SetParent(itemParent.transform);
            cubeRend = item.GetComponent<Renderer>();
            cubeRend.material.color = Color.yellow;
        }
    }

    void PlaceWalls()
    {
        GameObject wall01 = Instantiate(cubePrefab, new Vector3(cubePos.x / 2, cubePos.y, cubePos.z + .5f),
            Quaternion.identity);
        wall01.name = "Wall";
        wall01.transform.localScale = new Vector3(cubePos.x, wallsHeight, 1);

        cubeRend = wall01.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;

        GameObject wall02 = Instantiate(cubePrefab, new Vector3(cubePos.x + .5f, cubePos.y, cubePos.z / 2),
            Quaternion.identity);
        wall02.name = "Wall";
        wall02.transform.localScale = new Vector3(1, wallsHeight, cubePos.z);
        cubeRend = wall02.GetComponent<Renderer>();
        wall02.transform.SetParent(environmentParent.transform);
        cubeRend.material.color = Color.blue;

        GameObject wall03 = Instantiate(cubePrefab, new Vector3(cubePos.x / 2, cubePos.y, -.5f), Quaternion.identity);
        wall03.name = "Wall";
        wall03.transform.localScale = new Vector3(cubePos.x, wallsHeight, 1);
        wall03.transform.SetParent(environmentParent.transform);
        cubeRend = wall03.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;

        GameObject wall04 = Instantiate(cubePrefab, new Vector3(-.5f, cubePos.y, cubePos.z / 2), Quaternion.identity);
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
        ground.transform.localScale = new Vector3(cubePos.x, Mathf.Abs(-cubePos.y), cubePos.z);
        cubeRend = ground.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
    }

    public void RandomiseValues()
    {
        scale = Random.Range(5f, 8f);
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
            new Vector3(centrePos.x, (centrePos.y + scale) / 2, centrePos.z), Quaternion.identity);
        HumanHQprefab.transform.SetParent(HQParent.transform);
    }

    private void SpawnAlienHQPos()
    {
        alienPos = new Vector3(cubePos.x, cubePos.y + (scale / 2), cubePos.z);

        float tempAlienDist = Vector3.Distance(alienPos, prevAlienPos);

       //Debug.Log(tempAlienDist);

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
        }
    }
}