using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

public class MarcusTerrainTemp : MonoBehaviour
{
    public bool randomness;
    
    // Best mazes seem to come from 0.05 - 0.15
    // Larger zoom means tighter areas
    public int amount;
    public int aiLimit;

    Vector3 aiPos;
    int spawnedAI;

    Vector3 itemPos;
    float itemZoom;

    Vector3 floorPos;
    float zoom;
    Vector2 randomOffset;

    public GameObject pickup;
    public GameObject swarmer;
    
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject borderPrefab;
    
    public List<GameObject> bricks;
    public List<GameObject> items;
    public List<GameObject> aliens;
    
    // Start is called before the first frame update
    void Start()
    {
        if (randomness)
        {
            RandomiseValues();
        }
        else
        {
            GenerateMaze(0.1f, new Vector2(500, 500), 0.15f);
        }
    }

    public void Clear()
    {
        // Clear the list of tiles before regeneration
        foreach (GameObject tile in bricks)
        {
            Destroy(tile.gameObject);
        }
        bricks.Clear();
        // Clear the list of items before regeneration
        foreach (GameObject collectable in items)
        {
            Destroy(collectable.gameObject);
        }
        items.Clear();
        // Clear the current swarmer aliens
        foreach (GameObject ai in aliens)
        {
            Destroy(ai.gameObject);
        }

        if (randomness)
        {
            RandomiseValues();
        }
        else
        {
            GenerateMaze(0.1f, new Vector2(500, 500), 0.15f);
        }
    }

    void RandomiseValues()
    {
        zoom = Random.Range(0.08f, 0.12f);
        randomOffset.x = Random.Range(0, 1000);
        randomOffset.y = Random.Range(0, 1000);
        
        itemZoom = Random.Range(0.05f, 0.15f);
        
        GenerateMaze(zoom, randomOffset, itemZoom);
    }
    
    void GenerateMaze(float step, Vector2 startPoint, float itemStep)
    {
        // Spawn floor as one piece
        float floorPos = amount / 2f;

        GameObject floor = Instantiate(floorPrefab, new Vector3(floorPos, 0f, floorPos), Quaternion.identity);
        floor.transform.localScale = new Vector3(amount + 1, 0.1f, amount + 1);
        bricks.Add(floor);
        
        SpawnBorder();
        
        
        
        for (int x = 0; x < amount; x++)
        {
            for (int z = 0; z < amount; z++)
            {
	            float perlinRes = Mathf.PerlinNoise((x + startPoint.x) * step, (z + startPoint.y) * step);

	            if (perlinRes < 0.5f)
                {
	                Profiler.BeginSample("SpawnItems Test");
                    SpawnItems(x, z, itemStep);
                    Profiler.EndSample();
                }
                else
                {
                    GameObject wall = Instantiate(wallPrefab, new Vector3(x,perlinRes,z), Quaternion.identity);
                    bricks.Add(wall);
                }
            }
        }
    }

    void SpawnBorder()
    {
        GameObject curBorder;
        
        curBorder = Instantiate(borderPrefab, new Vector3(amount/2f, 2, -1), Quaternion.identity);
        curBorder.transform.localScale += new Vector3(amount, 0, 0);
        
        curBorder = Instantiate(borderPrefab, new Vector3(-1, 2, amount/2f), Quaternion.identity);
        curBorder.transform.localScale += new Vector3(0, 0, amount);
        
        curBorder = Instantiate(borderPrefab, new Vector3(amount/2f, 2, amount + 1), Quaternion.identity);
        curBorder.transform.localScale += new Vector3(amount, 0, 0);
        
        curBorder = Instantiate(borderPrefab, new Vector3(amount + 1, 2, amount/2f), Quaternion.identity);
        curBorder.transform.localScale += new Vector3(0, 0, amount);
    }

    void SpawnItems(float xValue, float zValue, float itemStep)
    {
        itemPos = new Vector3(xValue, 1f, zValue);
        
        if (Mathf.PerlinNoise(xValue * itemStep, zValue * itemStep) >= 0.7f)
        {
            GameObject item = Instantiate(pickup, itemPos, Quaternion.identity);
            items.Add(item);
        }
        else if (Mathf.PerlinNoise(xValue * itemStep, zValue * itemStep) <= 0.3f)
        {
            SpawnAI(xValue, zValue);
        }
    }

    void SpawnAI(float xPos, float zPos)
    {
        aiPos = new Vector3(xPos, 1f, zPos);
        
        if(spawnedAI < aiLimit)
        {
            GameObject aiInstance = Instantiate(swarmer, aiPos, Quaternion.identity);
            aliens.Add(aiInstance);
            spawnedAI++;
        }
    }
}