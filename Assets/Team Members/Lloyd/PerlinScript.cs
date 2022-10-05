using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinScript : MonoBehaviour
{

    public GameObject cubePrefab;

    private float x;
    private float y;

    public Vector3 position;



    private void Start()
    {
        for (int j = 0; j < 100; j++)
        {
            position.z = j;
            position.x = Mathf.PerlinNoise(Time.deltaTime * j * 5, 10) * 20;
            Instantiate(cubePrefab, position, Quaternion.identity);
            for (int i = 0; i < 100; i++)
            {
                position.x = i;
                position.y = Mathf.PerlinNoise(Time.deltaTime * i * 5, 10) * 20;
                Instantiate(cubePrefab, position, Quaternion.identity);

            }
        }
    }
}