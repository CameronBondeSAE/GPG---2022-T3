using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SpawnNoise : MonoBehaviour
{
    public GameObject Prefab;
    
    private List<GameObject> cubeLand = new List<GameObject>();

    public int amount;

    private float scale = 3f;
    private float zoomX;
    private float zoomZ;
    private float zoom;

    public bool randomMap;
    
    private float x;
    private float y;
    private Vector3 prefabPosition;

    
    private int item = 3;
    
    GameObject CubeParent;

    public void Start()
    {
        CubeParent = new GameObject("CubeParent");
        if (randomMap == true)
        {
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
        }
        else if (randomMap == false)
        {
            zoomX = 0.2f;
            zoomZ = 0.2f;
        }
    }

    public void ResetTheMap()
    {
        for (int cubes = 0; cubes < cubeLand.Count; cubes++)
        {
            Destroy(cubeLand[cubes].gameObject);
        }
        cubeLand.Clear();
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
        
        for (int positionX = 0; positionX < amount; positionX++)
        { 
            for (int positionZ = 0; positionZ < amount; positionZ++)
            {
                for (int positionY = 0; positionY < amount; positionY++)
                {
                    float perlinValue = Mathf.PerlinNoise((positionX * zoomX), (positionZ * zoomZ));
                    
                    prefabPosition.x = positionX;
                    prefabPosition.y = perlinValue * scale;
                    prefabPosition.z = positionZ;
                    
                    if (perlinValue > .3)
                    {
                        if (perlinValue > .5)
                        {
                            GameObject newCube = Instantiate(Prefab, prefabPosition, Quaternion.identity);
                            newCube.transform.SetParent(CubeParent.transform);
                            cubeLand.Add(newCube.gameObject);
                            newCube.GetComponent<Renderer>().material.color = Color.black;
                            if (perlinValue > .8)
                            {
                                newCube.GetComponent<Renderer>().material.color = Color.red;
                            }
                        }
                    }
                }
            }
        }
    }

}
