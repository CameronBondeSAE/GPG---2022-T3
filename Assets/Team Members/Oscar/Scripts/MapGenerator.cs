using System;
using System.Collections.Generic;
using Lloyd;
using Luke;
using Oscar;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : NetworkBehaviour, ILevelGenerate
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
    public GameObject borderParent;
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

    private void Awake()
    {
	    // Register my generator with the main GameManager because they call everything
	    GameManager.singleton.LevelGenerator = this;
	    
	    CubeParent = new GameObject("CubeParent");
	    BarrelParent = new GameObject("ItemParent");
	    AIParent = new GameObject("AIParent");
	    borderParent = new GameObject("borderParent");
	    HQParent = new GameObject("HQParent");
	    ItemParent = new GameObject("itemParent");
	    
	    //GameManager.singleton.OnGameStart += Spawner;
	    // GameManager.singleton.OnGameEnd += DeleteMap;
        
        
	    if (randomMap == true)
	    {
		    //randoms
		    zoomX = Random.Range(0.1f, 0.3f);
		    zoomZ = Random.Range(0.1f, 0.3f);
		    PrecalculateTerrain(zoomX,zoomZ);
	    }
	    else if (randomMap == false)
	    {
		    //standard averages
		    zoomX = 0.15f;
		    zoomZ = 0.15f;
		    PrecalculateTerrain(zoomX, zoomZ);
	    }
    }

    //OLLIE HACK: Need this to allow lobby level previews
    private void OnDisable()
    {
        GameManager.singleton.LevelGenerator = null;
    }

    public void Start()
    {

    }
    
    public void Spawner()
    {
        if (randomMap == true)
        {
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            PrecalculateTerrain(zoomX, zoomZ);
        }
        else if (randomMap == false)
        {
            PrecalculateTerrain(zoomX, zoomZ);
        }
    }

    public void PrecalculateTerrain(float zoomX, float zoomZ)
    {
        //Luke.GameManager.singleton.LevelFinishedLoading();
        
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
                        //GameManager.singleton.SpawnPerlinFinished();
                    }
                    else
                    {
                        totalAI.Add(prefabPosition);
                        //GameManager.singleton.SpawnAIFinished();
                        
                        totalExplosives.Add(prefabPosition);
                        //GameManager.singleton.SpawnExplosivesFinished();
                    }
                }
                else
                {
                    totalItems.Add(prefabPosition);
                    //GameManager.singleton.SpawnItemsFinished();
                }
                
                if (perlinValue < .4f && prefabPosition.x > 20 && prefabPosition.z > 20)
                {
                    totalHQ.Add(prefabPosition);
                    //GameManager.singleton.SpawnBasesFinished();
                }
            }
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
    
    public void SpawnPerlinClientRpc()
    {
        spawnEnvironment.SpawnPerlinWalls(totalCubes, CubeParent, perlinValue);
        //GameManager.singleton.SpawnPerlinFinished();
    }

    [ClientRpc]
    public void SpawnBorderClientRpc()
    {
        spawnEnvironment.SpawnTheEnvironment(prefabPosition, amount, scale, borderParent);
        //GameManager.singleton.SpawnBorderFinished();
    }

    [ClientRpc]
    public void SpawnAIClientRpc()
    {
        //spawnAI.SpawnAIInTheMaze(totalAI,AIParent, perlinValue);
        GameManager.singleton.spawnManager.SpawnSwarmerAI();
        //GameManager.singleton.SpawnAIFinished();
    }

    [ClientRpc]
    public void SpawnItemsClientRpc()
    {
        spawnItems.SpawnTheItems(totalItems, ItemParent);
        //GameManager.singleton.SpawnItemsFinished();
    }

    [ClientRpc]
    public void SpawnExplosivesClientRpc()
    {
        spawnExplosives.SpawningTheExplosives(totalExplosives, BarrelParent, perlinValue);
        //GameManager.singleton.SpawnExplosivesFinished();
    }

    [ClientRpc]
    public void SpawnBasesClientRpc()
    {
        spawnBases.SpawnTheBase(totalHQ, HQParent);
        //GameManager.singleton.SpawnBasesFinished();
    }
}
