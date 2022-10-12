using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawning : MonoBehaviour
{
    public GameObject pickup;
    public MarcusTerrain perlin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItems()
    {
        print("spawning items");
        int spawnChance = Random.Range(0, 10);
        
        if (perlin.brickPosition.y < 0.5f)
        {
            Instantiate(pickup, perlin.brickPosition, Quaternion.identity);
        }
    }
}
