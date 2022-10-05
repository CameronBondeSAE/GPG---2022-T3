using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinThings : MonoBehaviour
{
    Vector3 brickPosition;
    public GameObject caveBrickPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 100; x++)
        {
            brickPosition.x = x;
            brickPosition.y = Mathf.PerlinNoise(Time.deltaTime * x,0) * 100;
            
            Instantiate(caveBrickPrefab, brickPosition, Quaternion.identity);

            for (int i = 0; i < 100; i++)
            {
                brickPosition.x = i;
                brickPosition.z = Mathf.PerlinNoise(Time.deltaTime * i, 0) * 100;

                Instantiate(caveBrickPrefab, brickPosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
