using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarcusTerrain : MonoBehaviour
{
    // Best mazes seem to come from 0.05 - 0.15
    // Larger zoom means tighter areas
    public int amount;
    float zoom;
    Vector2 randomOffset;

    Vector3 itemPos;
    Vector3 brickPosition;
    Vector3 floorPos;

    public GameObject pickup;
    
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    
    public List<GameObject> bricks;
    public List<GameObject> items;
    
    // Start is called before the first frame update
    void Start()
    {
        RandomiseValues();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        RandomiseValues();
    }

    void RandomiseValues()
    {
        zoom = Random.Range(0.08f, 0.12f);
        randomOffset.x = Random.Range(0, 1000);
        randomOffset.y = Random.Range(0, 1000);
        
        GenerateMaze(zoom, randomOffset);
    }
    
    void GenerateMaze(float step, Vector2 startPoint)
    {
        for (int x = 0; x < amount; x++)
        {
            for (int z = 0; z < amount; z++)
            {
                brickPosition.x = x;
                brickPosition.y = Mathf.PerlinNoise((x + startPoint.x) * step, (z + startPoint.y) * step);
                brickPosition.z = z;

                if (brickPosition.y < 0.5f)
                {
                    floorPos = new Vector3(brickPosition.x, 0, brickPosition.z);

                    GameObject go = Instantiate(floorPrefab, floorPos, Quaternion.identity);
                    bricks.Add(go);
                    
                    SpawnItems(x, z);
                }
                else
                {
                    GameObject go = Instantiate(wallPrefab, brickPosition, Quaternion.identity);
                    bricks.Add(go);
                }
            }
        }
    }

    void SpawnItems(float xValue, float zValue)
    {
        float itemZoom = 0.15f;
        itemPos = new Vector3(xValue, 1f, zValue);
        
        if (Mathf.PerlinNoise(xValue * itemZoom, zValue * itemZoom) >= 0.7f)
        {
            GameObject item = Instantiate(pickup, itemPos, Quaternion.identity);
            items.Add(item);
        }
    }
}
