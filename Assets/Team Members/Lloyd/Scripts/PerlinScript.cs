using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PerlinScript : MonoBehaviour
{
    [Header("Terrain Object")] public GameObject cubePrefab;

    [Header("Item Prefab")] [SerializeField]
    private GameObject itemPrefab;

    private Vector3 itemPos;

    [SerializeField] private int numItems;

    

    [Header("Noise Settings")] [SerializeField]
    private int numCube;
    
    [SerializeField] private float zoomX;
    [SerializeField] private float zoomY;
    [SerializeField] private float scale;

    [Header("Parent Objs (fix later)")] [SerializeField]
    public GameObject terrainParent;
    public GameObject itemParent;
    public GameObject environmentParent;
    

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
    }
    
    

    void SpawnItems()
    {
        itemPos = new Vector3(cubePos.x, 1, cubePos.z);
        
        float a = -.5f;
        float b = .5f;
        float perlinNoise = Mathf.PerlinNoise(a, b * scale);

        if (perlinNoise > 0.0f)
        {
            GameObject item =
                Instantiate(itemPrefab, itemPos, Quaternion.identity) as GameObject;
            item.transform.SetParent(itemParent.transform);
            cubeRend = item.GetComponent<Renderer>();
            cubeRend.material.color = Color.yellow;
        }
    }

    void PlaceWalls()
    {
        
        GameObject wall01 = Instantiate(cubePrefab, new Vector3(cubePos.x/2, cubePos.y, cubePos.z+.5f), Quaternion.identity);
        wall01.name = "Wall01";
        wall01.transform.localScale = new Vector3(cubePos.x, cubePos.y*scale*2, 1);
       
        cubeRend = wall01.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
        
        GameObject wall02 = Instantiate(cubePrefab, new Vector3(cubePos.x+.5f, cubePos.y, cubePos.z/2), Quaternion.identity);
        wall02.name = "Wall02";
        wall02.transform.localScale = new Vector3(1, cubePos.y*scale*2, cubePos.z);
        cubeRend = wall02.GetComponent<Renderer>();
        wall02.transform.SetParent(environmentParent.transform);
        cubeRend.material.color = Color.blue;
        
        GameObject wall03 = Instantiate(cubePrefab, new Vector3(cubePos.x/2, cubePos.y, -.5f), Quaternion.identity);
        wall03.name = "Wall03";
        wall03.transform.localScale = new Vector3(cubePos.x, cubePos.y*scale*2, 1);
        wall03.transform.SetParent(environmentParent.transform);
        cubeRend = wall03.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
        
        GameObject wall04 = Instantiate(cubePrefab, new Vector3(-.5f, cubePos.y, cubePos.z/2), Quaternion.identity);
        wall04.name = "Wall04";
        wall04.transform.localScale = new Vector3(1, cubePos.y*scale*2, cubePos.z);
        wall04.transform.SetParent(environmentParent.transform);
        cubeRend = wall04.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
        
        
       
    }

    void PlaceGround()
    {
        GameObject ground = Instantiate(cubePrefab, new Vector3(cubePos.x / 2, 0, cubePos.z / 2), Quaternion.identity);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(cubePos.x, cubePos.y, cubePos.z);
        cubeRend = ground.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
    }

    public void RandomiseValues()
    {
        scale = Random.Range(5f, 8f);
        zoomX = Random.Range(.07f, .2f);
        zoomY = Random.Range(.07f, .2f);
    }
}