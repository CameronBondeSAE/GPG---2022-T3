using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcusTerrain : MonoBehaviour
{
    // Best mazes seem to come from 0.05 - 0.15
    // Larger zoom means tighter areas
    public float zoom;
    Vector3 randomOffset;

    Vector3 brickPosition;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    
    public List<GameObject> bricks;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearPrevious()
    {
        foreach (GameObject item in bricks)
        {
            Destroy(item.gameObject);
        }
        bricks.Clear();
        
        GenerateMaze();
    }
    
    void GenerateMaze()
    {
        randomOffset.x = Random.Range(0, 1000);
        randomOffset.z = Random.Range(0, 1000);
        
        for (int x = 0; x < 100; x++)
        {
            for (int z = 0; z < 100; z++)
            {
                brickPosition.x = x;
                brickPosition.y = Mathf.PerlinNoise((x + randomOffset.x) * zoom, (z + randomOffset.z) * zoom);
                brickPosition.z = z;

                if (brickPosition.y < 0.5f)
                {
                    GameObject go = Instantiate(floorPrefab, brickPosition, Quaternion.identity);
                    bricks.Add(go);
                }
                else
                {
                    GameObject go = Instantiate(wallPrefab, brickPosition, Quaternion.identity);
                    bricks.Add(go);
                }
            }
        }
    }
}
