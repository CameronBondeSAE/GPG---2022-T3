using System;
using System.Collections.Generic;
using Luke;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public Oscar.SpawnBases spawnBases;
    public Oscar.SpawnEnvironment spawnEnvironment;
    public Oscar.SpawnExplosives spawnExplosives;
    public Oscar.SpawnAI spawnAI;
    public Oscar.SpawnItems spawnItems;
    
    //all the spawned object parents, and making them fit nicely in the hierarchy
    GameObject CubeParent; 
    GameObject BarrelParent;
    GameObject AIParent;
    GameObject borderParent;
    GameObject HQParent;
    GameObject ItemParent;
    
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

    private float perlinValue;
    
    //arrays for the spawning to make it event driven
    private List<Vector3> totalAI = new List<Vector3>();
    private List<Vector3> totalItems = new List<Vector3>();
    private List<Vector3> totalExplosives = new List<Vector3>();
    private List<Vector3> totalHQ = new List<Vector3>();


    public void Start()
    {
        GameManager.singleton.OnGameStart += Spawner;
        GameManager.singleton.OnGameEnd += DeleteMap;
        
        CubeParent = new GameObject("CubeParent");
        BarrelParent = new GameObject("ItemParent");
        AIParent = new GameObject("AIParent");
        borderParent = new GameObject("borderParent");
        HQParent = new GameObject("HQParent");
        ItemParent = new GameObject("itemParent");
        
        if (randomMap == true)
        {
            //randoms
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            spawnTerrain(zoomX,zoomZ);
        }
        else if (randomMap == false)
        {
            //standard averages
            zoomX = 0.15f;
            zoomZ = 0.15f;
            spawnTerrain(zoomX,zoomZ);
        }
    }

    public void ResetTheMap()
    {
        Destroy(CubeParent);
        Destroy(BarrelParent);
        Destroy(AIParent);
        Destroy(borderParent);
        Destroy(HQParent);
        Destroy(ItemParent);
        
        CubeParent = new GameObject("CubeParent");
        BarrelParent = new GameObject("ItemParent");
        AIParent = new GameObject("AIParent");
        borderParent = new GameObject("borderParent");
        HQParent = new GameObject("HQParent");
        ItemParent = new GameObject("itemParent");

        //reset the values so bases will respawn
        spawnBases.HQAmount = 0;
        spawnBases.tempBaseDist = 0;
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
                perlinValue = Mathf.PerlinNoise((positionX * zoomX), (positionZ * zoomZ));
                
                prefabPosition.x = positionX;
                prefabPosition.y = perlinValue * scale;
                prefabPosition.z = positionZ;
                
                if (perlinValue > .3)
                {
                    if (perlinValue > .5)
                    {
                        spawnEnvironment.SpawnPerlinWalls(prefabPosition, CubeParent, perlinValue);
                    }
                    else
                    {
                        totalAI.Add(prefabPosition);
                        //spawnAI.SpawnAIInTheMaze(prefabPosition,AIParent, perlinValue);
                        totalExplosives.Add(prefabPosition);
                        //spawnExplosives.SpawningTheExplosives(prefabPosition,BarrelParent, perlinValue);
                    }
                }
                else
                {
                    totalItems.Add(prefabPosition);
                    //spawnItems.SpawnTheItems(prefabPosition, ItemParent, perlinValue);
                }
                
                if (perlinValue < .4f && prefabPosition.x > 20 && prefabPosition.z > 20)
                {
                    totalHQ.Add(prefabPosition);
                    //spawnBases.SpawnTheBase(prefabPosition, HQParent);
                }
            }
        }
        spawnEnvironment.SpawnTheEnvironment(prefabPosition, amount, scale, borderParent);
    }
    
    public void AI()
    {
        spawnAI.SpawnAIInTheMaze(totalAI,AIParent, perlinValue);
    }

    public void Explosives()
    {
        spawnExplosives.SpawningTheExplosives(totalExplosives, BarrelParent, perlinValue);
    }

    public void Items()
    {
        spawnItems.SpawnTheItems(totalItems, ItemParent, perlinValue);
    }

    public void HQ()
    {
        spawnBases.SpawnTheBase(totalHQ, HQParent);
    }
    
    
    private void DeleteMap()
    {
        Destroy(CubeParent);
        Destroy(BarrelParent);
        Destroy(AIParent);
        Destroy(borderParent);
        Destroy(HQParent);
        Destroy(ItemParent);
        
        CubeParent = new GameObject("CubeParent");
        BarrelParent = new GameObject("ItemParent");
        AIParent = new GameObject("AIParent");
        borderParent = new GameObject("borderParent");
        HQParent = new GameObject("HQParent");
        ItemParent = new GameObject("itemParent");

        
        //reset values so bases can respawn
        spawnBases.HQAmount = 0;
        spawnBases.tempBaseDist = 0;
    }
}
