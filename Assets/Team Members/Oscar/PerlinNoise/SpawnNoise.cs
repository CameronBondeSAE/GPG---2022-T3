using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnNoise : MonoBehaviour
{
    public GameObject Prefab;
    public List<GameObject> cubeLand = new List<GameObject>();

    public int amount;

    public float scale;
    private float zoomX;
    private float zoomZ;

    private float timer;
    private float interval = 1f;
    private float x;
    private float y;
    private Vector3 prefabPosition;

    private void Start()
    {
        scale = Random.Range(1f,10f);
        zoomX = Random.Range(0.05f, 0.1f);
        zoomZ = Random.Range(0.05f, 0.1f);
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
