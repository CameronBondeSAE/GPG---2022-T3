using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnNoise : MonoBehaviour
{
    public GameObject Prefab;
    public List<GameObject> cubeLand = new List<GameObject>();

    public int amount;

    public float scale;
    public float zoom;

    private float timer;
    private float interval = 1f;
    private float x;
    private float y;
    private Vector3 prefabPosition;

    private void Start()
    {
        for (int positionX = 0; positionX < amount; positionX++)
        { 
            for (int positionZ = 0; positionZ < amount; positionZ++)
            {
                for (int positionY = 0; positionY < amount; positionY++)
                {
                    prefabPosition.x = Mathf.PerlinNoise((positionZ * zoom), (positionZ * zoom)) * positionX;
                    prefabPosition.y = Mathf.PerlinNoise((positionX * zoom), (positionZ * zoom)) * positionY;
                    prefabPosition.z = Mathf.PerlinNoise((positionX * zoom), (positionY * zoom)) * positionZ;
                    GameObject newCube = Instantiate(Prefab, prefabPosition, Quaternion.identity);
                    cubeLand.Add(newCube.gameObject);
                }
                
                
            }
        }
    }

}
