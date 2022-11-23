using System;
using System.Collections.Generic;
using Lloyd;
using Luke;
using Oscar;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour, ILevelGenerate
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
    private List<Vector3> totalCubes = new List<Vector3>();
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
            SpawnTerrain(zoomX,zoomZ);
        }
        else if (randomMap == false)
        {
            //standard averages
            zoomX = 0.15f;
            zoomZ = 0.15f;
            SpawnTerrain(zoomX,zoomZ);
        }
    }
    
    public void Spawner()
    {
        if (randomMap == true)
        {
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            SpawnTerrain(zoomX, zoomZ);
        }
        else if (randomMap == false)
        {
            SpawnTerrain(zoomX, zoomZ);
        }
    }

    public void SpawnTerrain(float zoomX, float zoomZ)
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
                        totalCubes.Add(prefabPosition);
                    }
                    else
                    {
                        totalAI.Add(prefabPosition);
                        totalExplosives.Add(prefabPosition);
                    }
                }
                else
                {
                    totalItems.Add(prefabPosition);
                }
                
                if (perlinValue < .4f && prefabPosition.x > 20 && prefabPosition.z > 20)
                {
                    totalHQ.Add(prefabPosition);
                }
            }
        }
        Luke.GameManager.singleton.LevelFinishedLoading();
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
            
            totalCubes = new List<Vector3>();
            totalAI = new List<Vector3>();
            totalItems = new List<Vector3>();
            totalExplosives = new List<Vector3>();
            totalHQ = new List<Vector3>();

            //reset the values so bases will respawn
            spawnBases.HQAmount = 0;
            spawnBases.tempBaseDist = 0;
            Spawner();
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

        totalCubes = new List<Vector3>();
        totalAI = new List<Vector3>();
        totalItems = new List<Vector3>();
        totalExplosives = new List<Vector3>();
        totalHQ = new List<Vector3>();
        
        //reset values so bases can respawn
        spawnBases.HQAmount = 0;
        spawnBases.tempBaseDist = 0;
    }

    public void SpawnPerlin()
    {
        spawnEnvironment.SpawnPerlinWalls(totalCubes, CubeParent, perlinValue);
    }

    public void SpawnBorder()
    {
        spawnEnvironment.SpawnTheEnvironment(prefabPosition, amount, scale, borderParent);
    }

    public void SpawnAI()
    {
        spawnAI.SpawnAIInTheMaze(totalAI,AIParent, perlinValue);
    }

    public void SpawnItems()
    {
        spawnItems.SpawnTheItems(totalItems, ItemParent);
    }

    public void SpawnExplosives()
    {
        spawnExplosives.SpawningTheExplosives(totalExplosives, BarrelParent, perlinValue);
    }

    public void SpawnBases()
    {
        spawnBases.SpawnTheBase(totalHQ, HQParent);
    }
}
