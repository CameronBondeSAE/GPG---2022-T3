using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinThings : MonoBehaviour
{
    public float zoom;
    public float scale;
    
    Vector3 brickPosition;
    public GameObject caveBrickPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 100; x++)
        {
            for (int z = 0; z < 100; z++)
            {
                brickPosition.x = x;
                brickPosition.y = Mathf.PerlinNoise(x * zoom,z * zoom) * scale;
                brickPosition.z = z;
                
                Instantiate(caveBrickPrefab, brickPosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
