using System;
using System.Collections.Generic;
using Luke;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    //all the spawned objects, and making them fit nicely in the hierarchy
    public GameObject cubePrefab;
    public GameObject Barrel;
    public GameObject AISpawner;
    public GameObject Base;
    public GameObject borderPrefab;
        
    GameObject CubeParent; 
    GameObject BarrelParent;
    GameObject AIParent;
    GameObject borderParent;
    GameObject BaseParent;
    GameObject HQParent;
    


    //Base spawning variables
    [SerializeField] 
    private int HQAmount = 0;

    [SerializeField] 
    private float LastBaseDistance;

    private Vector3 prevBasePos;
    
    [SerializeField]
    private float minDist = 90;

    private float tempBaseDist;
    

    //Perlin noise values and required elements to spawn the maze.
    public int amount;

    private float scale = 3f;
    private float zoomX;
    private float zoomZ;
    private float zoom;

    public bool randomMap;
    
    private float x;
    private float y;
    private Vector3 prefabPosition;
    public int cubeSize = 1;

    public void Start()
    {
        
        GameManager.singleton.OnGameStart += Spawner;
        GameManager.singleton.OnGameEnd += DeleteMap;
        
        CubeParent = new GameObject("CubeParent");
        BarrelParent = new GameObject("ItemParent");
        AIParent = new GameObject("AIParent");
        borderParent = new GameObject("borderParent");
        HQParent = new GameObject("HQParent");
        
        if (randomMap == true)
        {
            //randoms
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            //spawnTerrain(zoomX,zoomZ);
        }
        else if (randomMap == false)
        {
            //standard averages
            zoomX = 0.15f;
            zoomZ = 0.15f;
            //spawnTerrain(zoomX,zoomZ);
        }
    }

    public void ResetTheMap()
    {
        Destroy(CubeParent);
        Destroy(BarrelParent);
        Destroy(AIParent);
        Destroy(borderParent);
        Destroy(HQParent);
        
        CubeParent = new GameObject("CubeParent");
        BarrelParent = new GameObject("ItemParent");
        AIParent = new GameObject("AIParent");
        borderParent = new GameObject("borderParent");
        HQParent = new GameObject("HQParent");

        //reset the values so bases will respawn
        HQAmount = 0;
        tempBaseDist = 0;
        Spawner();
    }

    public void Spawner()
    {
        if (randomMap == true)
        {
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            spawnTerrain(zoomX, zoomZ);
        }
        else if (randomMap == false)
        {
            spawnTerrain(zoomX, zoomZ);
        }
    }
    
    public void spawnTerrain(float zoomX, float zoomZ)
    {
        print("scale = " + scale + " zoomX = " + zoomX + " zoomZ = " + zoomZ);
        
        for (int positionX = 0; positionX < amount; positionX=positionX+cubeSize)
        { 
            for (int positionZ = 0; positionZ < amount; positionZ=positionZ+cubeSize)
            {
                float perlinValue = Mathf.PerlinNoise((positionX * zoomX), (positionZ * zoomZ));
                
                prefabPosition.x = positionX;
                prefabPosition.y = perlinValue * scale;
                prefabPosition.z = positionZ;
                
                if (perlinValue > .3)
                {
                    if (perlinValue > .5)
                    {
                        GameObject newCube = Instantiate(cubePrefab, prefabPosition, Quaternion.identity);
                        newCube.transform.SetParent(CubeParent.transform);
                        newCube.GetComponent<Renderer>().material.color = Color.black;
                        if (perlinValue > .8)
                        {
                            newCube.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                    else
                    {
                        if (perlinValue < .4f && prefabPosition.x > 20 && prefabPosition.z > 20)
                        {
                            SpawnTheBase(prefabPosition);
                        }
                        
                        SpawnAIInTheMaze(prefabPosition);
                        SpawningTheBarrels(prefabPosition);
                        //SpawningTheItems(prefabPosition);
                    }
                }
            }
        }
        
        SpawnWalls();
    }

    void SpawnTheBase(Vector3 prefabPosition)
    {
        //minDist = 90 - (amount / 2);
        //learn how to calculate distances between potential created object and the previously spawned one
        tempBaseDist = Vector3.Distance(prefabPosition, prevBasePos);
        
        //if the distance is less then the minimum distance set 
        if (tempBaseDist > minDist)
        {
            //and there isnt already 2 bases
            if (HQAmount <= 1)
            {
                //spawn the bases
                Vector3 tempBasePos = prefabPosition;
                prevBasePos = tempBasePos;
                GameObject HQ = Instantiate(Base,prefabPosition,quaternion.identity);
                HQ.name = "base";
                HQ.transform.SetParent(HQParent.transform);
                HQAmount++;
            }
        }
    }

    void SpawningTheBarrels(Vector3 prefabPosition)
    {
        //use new perlin to create clusters of barrels
        
        int SpawmTheBarrels = Random.Range(1, 50);
        if (SpawmTheBarrels == 1)
        {
            GameObject spawnedItem = Instantiate(Barrel, prefabPosition, quaternion.identity);
            OnCollisionEnter(spawnedItem.GetComponent<Collider>());
            void OnCollisionEnter(Collider collision)
            {
                if (collision.gameObject.name == "base")
                {
                    Destroy(spawnedItem);
                }
                else
                {
                    spawnedItem.transform.SetParent(BarrelParent.transform);
                    spawnedItem.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
            
        }
    }

    void SpawnAIInTheMaze(Vector3 prefabPosition)
    {
        //create spawn location with new perlin then 

        int spawnTheAI = Random.Range(1, 50);
        if (spawnTheAI == 1)
        {
            GameObject spawnedAI = Instantiate(AISpawner, prefabPosition, quaternion.identity);
            OnCollisionEnter(spawnedAI.GetComponent<Collider>());

            void OnCollisionEnter(Collider collision)
            {
                if (collision.gameObject.name == "base")
                {
                    Destroy(spawnedAI);
                }
                else
                {
                    spawnedAI.transform.SetParent(AIParent.transform);
                }
            }
        }
    }

    void SpawnWalls()
    {
        GameObject firstWall = Instantiate(borderPrefab,
            new Vector3(0, prefabPosition.y, (amount / 2)), quaternion.identity);
        firstWall.name = "firstWall";
        firstWall.transform.localScale = new Vector3(1, prefabPosition.y * scale * 2, prefabPosition.z);
        firstWall.GetComponent<Renderer>().material.color = Color.black;
        firstWall.transform.SetParent(borderParent.transform);

        
        GameObject secondWall = Instantiate(borderPrefab,
            new Vector3(prefabPosition.x - (amount / 2),prefabPosition.y,prefabPosition.z+1), Quaternion.identity);
        secondWall.name = "secondWall";
        secondWall.transform.localScale = new Vector3(prefabPosition.x,prefabPosition.y * scale * 2,1);
        secondWall.GetComponent<Renderer>().material.color = Color.black;
        secondWall.transform.SetParent(borderParent.transform);
        
        
        GameObject thirdWall = Instantiate(borderPrefab,
            new Vector3(prefabPosition.x+1, prefabPosition.y, prefabPosition.z - (amount / 2)), quaternion.identity);
        thirdWall.name = "thirdWall";
        thirdWall.transform.localScale = new Vector3(1, prefabPosition.y * scale * 2, prefabPosition.z);
        thirdWall.GetComponent<Renderer>().material.color = Color.black;
        thirdWall.transform.SetParent(borderParent.transform);
        
        
        GameObject fourthWall = Instantiate(borderPrefab,
            new Vector3((amount / 2), prefabPosition.y, 0), quaternion.identity);
        fourthWall.name = "fourthWall";
        fourthWall.transform.localScale = new Vector3(prefabPosition.x, prefabPosition.y * scale * 2, 1);
        fourthWall.GetComponent<Renderer>().material.color = Color.black;
        fourthWall.transform.SetParent(borderParent.transform);
        
        
        GameObject floor = Instantiate(borderPrefab,
            new Vector3((amount / 2), 0, (amount / 2)), quaternion.identity);
        floor.name = "floor";
        floor.transform.localScale = new Vector3(amount, 1, amount);
        floor.GetComponent<Renderer>().material.color = Color.green;
        floor.transform.SetParent(borderParent.transform);
    }
    
    private void DeleteMap()
    {
        Destroy(CubeParent);
        Destroy(BarrelParent);
        Destroy(AIParent);
        Destroy(borderParent);
        Destroy(HQParent);
        
        CubeParent = new GameObject("CubeParent");
        BarrelParent = new GameObject("ItemParent");
        AIParent = new GameObject("AIParent");
        borderParent = new GameObject("borderParent");
        HQParent = new GameObject("HQParent");
        
        //reset values so bases can respawn
        HQAmount = 0;
        tempBaseDist = 0;
    }
    
    private void SpawningTheItems(Vector3 prefabPosition)
    {
        
    }
}

    