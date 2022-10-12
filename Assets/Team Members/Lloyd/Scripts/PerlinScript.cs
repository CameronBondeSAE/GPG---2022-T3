using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerlinScript : MonoBehaviour
{
    public GameObject cubePrefab;

    private GameObject cubeSpawned;

    [Header("Noise Settings")]
    [SerializeField] private int cubeNum;
    [SerializeField] private float zoomX;
    [SerializeField] private float zoomY;
    [SerializeField] private float scale;

    public GameObject parentObj;

    private Vector3 cubePos;
    private Renderer cubeRend;

    private float goPos;

    private bool canDie;
    
    

    void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
        {
            GenerateTerrain();
        }
    }

    public void GenerateTerrain()
    {
        EventManager.TerrainClearFunction();
        SpawnTerrain();
    }

    void SpawnTerrain()
    {
        scale = Random.Range(10f, 20f);
        zoomX = Random.Range(.1f, .3f);
        zoomY = Random.Range(.1f, .3f);

        
        for (int x = 0; x < cubeNum; x++)
        {
            for (int z = 0; z < cubeNum; z++)
            {
                cubePos.x = x;
                float perlinNoise = Mathf.PerlinNoise(x * zoomX,z * zoomY);
                cubePos.y = perlinNoise * scale;
                cubePos.z = z;

                if (perlinNoise > 0.4)
                {
                    GameObject go = Instantiate(cubePrefab, cubePos, Quaternion.identity) as GameObject;
                    go.transform.SetParent(parentObj.transform);
                    
                    
                                 
                                 if (perlinNoise > .6f)
                                 {
                                     cubeRend = go.GetComponent<Renderer>();
                                     cubeRend.material.color = Color.red;
                                 }

                                 if (perlinNoise < .5f)
                                 {
                                     cubeRend = go.GetComponent<Renderer>();
                                     cubeRend.material.color = Color.black;
                                 }
                                 
                                 // for  
                }

            }
        }
       
    }
    
    
}