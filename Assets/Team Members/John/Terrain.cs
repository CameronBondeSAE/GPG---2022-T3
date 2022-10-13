using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class Terrain : MonoBehaviour
{
    public int cubes = 0;
    //How many blocks will the map have
    //(This includes along the x and z axis aka if it's set to 20 that means 20 block of the x
    //and 20 blocks along the z axis)
    
    public float perlinNoise = 0f;
    //Leave this on 0f, this is just to show the value of the Mathf.PerlinNoise function
    
    public float refinement = 0f;
    //0.1f is a good amount, the more you up this value the closer the mounds
    //get to each other and the less natural they look
    
    public float multiplier = 0f;
    // this effects the height of the mounds
    
    public float perlinCutOffThreshold = 0f;
    // this value is used to determine at what point do we not
    // spawn in cubes to make a traversable terrain  in between the mounds

    public GameObject parent;
    

    private void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        //GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        
        for (int z = 0; z < cubes; z++)
        {
            for (int x = 0; x < cubes; x++)
            {
                perlinNoise = Mathf.PerlinNoise(z * refinement, x * refinement);
                if (perlinNoise > perlinCutOffThreshold)
                {
                    GameObject mounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    mounds.transform.position = new Vector3(z, (perlinNoise-perlinCutOffThreshold) * multiplier, x);
                    mounds.transform.SetParent(parent.transform);

                }
            }   
        }
        
    }
    
}
