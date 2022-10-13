using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnNoise : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject ItemSpawner;
    
    private List<GameObject> cubeLand = new List<GameObject>();
    private List<GameObject> ItemList = new List<GameObject>();

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
    
    private int item = 3;
    
    GameObject CubeParent;
    GameObject ItemParent;
    
    public void Start()
    {
        CubeParent = new GameObject("CubeParent");
        ItemParent = new GameObject("ItemParent");
        if (randomMap == true)
        {
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            spawnRandomTerrain(zoomZ, zoomX, scale);
        }
        else if (randomMap == false)
        {
            zoomX = 0.15f;
            zoomZ = 0.15f;
            spawnRandomTerrain(zoomZ, zoomX, scale);
        }
    }

    public void ResetTheMap()
    {
        for (int cubes = 0; cubes < cubeLand.Count; cubes++)
        {
            Destroy(cubeLand[cubes].gameObject);
        }
        cubeLand.Clear();
        for (int items = 0; items < ItemList.Count; items++)
        {
            Destroy(ItemList[items].gameObject);
        }
        ItemList.Clear();
        spawner();
    }

    public void spawner()
    {
        if (randomMap == true)
        {
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            spawnRandomTerrain(zoomX, zoomZ, scale);
        }
        else if (randomMap == false)
        {
            spawnRandomTerrain(zoomX, zoomZ, scale);
        }
    }
    
    public void spawnRandomTerrain(float zoomX, float zoomZ, float scale)
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
                        cubeLand.Add(newCube.gameObject);
                        newCube.GetComponent<Renderer>().material.color = Color.black;
                        if (perlinValue > .8)
                        {
                            newCube.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                    else
                    {
                        int randomChance = Random.Range(1, 100);
                        if (randomChance == 1)
                        {
                            GameObject spawnedItem = Instantiate(ItemSpawner, prefabPosition, quaternion.identity);
                            
                            spawnedItem.transform.SetParent(ItemParent.transform);
                            ItemList.Add(spawnedItem.gameObject);
                            
                            spawnedItem.GetComponent<Renderer>().material.color = Color.blue;
                        }
                    }
                }
            }
        }
    }
}
