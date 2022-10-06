using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    public Transform otherThing;
    public GameObject caveBrickPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 100; x++)
        {
            Vector3 brickPosition;
            brickPosition.x = x;
            // brickPosition.y = // perin stuff
                // Instantiate(caveBrickPrefab, brickPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        otherThing.position = new Vector3(0, Mathf.PerlinNoise(Time.time,0), 0);
    }
}
