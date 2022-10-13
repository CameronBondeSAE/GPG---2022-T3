using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcusTerrain : MonoBehaviour
{
    // Best mazes seem to come from 0.05 - 0.15
    // Larger zoom means tighter areas
    float zoom;
    Vector2 randomOffset;

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
        SpawnItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearMaze()
    {
        foreach (GameObject tile in bricks)
        {
            Destroy(tile.gameObject);
        }
        bricks.Clear();
        
        RandomiseValues();
    }

    public void ClearItems()
    {
        foreach (GameObject collectable in items)
        {
            Destroy(collectable.gameObject);
        }
        items.Clear();
        
        SpawnItems();
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
        for (int x = 0; x < 100; x++)
        {
            for (int z = 0; z < 100; z++)
            {
                brickPosition.x = x;
                brickPosition.y = Mathf.PerlinNoise((x + startPoint.x) * step, (z + startPoint.y) * step);
                brickPosition.z = z;

                if (brickPosition.y < 0.5f)
                {
                    floorPos = new Vector3(brickPosition.x, 0, brickPosition.z);

                    GameObject go = Instantiate(floorPrefab, floorPos, Quaternion.identity);
                    bricks.Add(go);
                }
                else
                {
                    GameObject go = Instantiate(wallPrefab, brickPosition, Quaternion.identity);
                    bricks.Add(go);
                    
                    SpawnItems();
                }
            }
        }
    }

    public void SpawnItems()
    {
        
    }
}
