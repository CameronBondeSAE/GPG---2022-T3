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
    public List<GameObject> cubeLand = new List<GameObject>();

    public int amount;

    private float scale;
    private float zoomX;
    private float zoomZ;

    private float timer;
    private float interval = 1f;
    private float x;
    private float y;
    private Vector3 prefabPosition;
    
    private void Start()
    {
        spawnTerrain();
    }

    private void FixedUpdate()
    {
        if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
        {
            for (int cubes = 0; cubes < cubeLand.Count; cubes++)
            {
                Destroy(cubeLand[cubes].gameObject);
            }
            cubeLand.Clear();
            spawnTerrain();
        }
    }

    void spawnTerrain()
    {
        scale = Random.Range(2f,3f);
        zoomX = Random.Range(0.1f, 0.3f);
        zoomZ = Random.Range(0.1f, 0.3f);
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
                            cubeLand.Add(newCube.gameObject);
                        }
                    }
                }
            }
        }
    }
}
