using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerlinThings : MonoBehaviour
{
    public float zoom;
    public float scale;
    
    float updateTimer = 5f;
    Vector3 brickPosition;
    public GameObject caveBrickPrefab;

    public List<GameObject> cubes;

    // Start is called before the first frame update
    void Start()
    {
        ChangeTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer -= Time.deltaTime;

        if (updateTimer <= 0)
        {
            foreach (GameObject cube in cubes)
            {
                Destroy(cube.gameObject);
            }
            cubes.Clear();
            
            ChangeTerrain();
            updateTimer = 5f;
        }
    }

    void ChangeTerrain()
    {
        print("making terrain");
        
        zoom = Random.Range(0.01f, 0.2f);
        scale = Random.Range(1, 40);
        
        for (int x = 0; x < 100; x++)
        {
            for (int z = 0; z < 100; z++)
            {
                brickPosition.x = x;
                brickPosition.y = Mathf.PerlinNoise(x * zoom,z * zoom) * scale;
                brickPosition.z = z;
                
                GameObject go = Instantiate(caveBrickPrefab, brickPosition, Quaternion.identity);
                cubes.Add(go);
            }
        }
    }
}
