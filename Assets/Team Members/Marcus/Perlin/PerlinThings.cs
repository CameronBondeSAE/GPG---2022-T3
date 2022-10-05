using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinThings : MonoBehaviour
{
    public GameObject caveBrickPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            Vector3 brickPosition;
            brickPosition.x = x;
            brickPosition.y = Mathf.PerlinNoise(x,0);
            brickPosition.z = 0;
            
            Instantiate(caveBrickPrefab, brickPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
