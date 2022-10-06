using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerlinScript : MonoBehaviour
{

    public GameObject cubePrefab;

    private GameObject cubeSpawned;

    private float zoom;
    private float zoom1;
    private float scale;

    public Vector3 cubePos;
    private Renderer cubeRend;

    private float goPos;

    private bool canDie;

    void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
        {
            EventManager.TerrainClearFunction();
            SpawnTerrain();
        }
    }

    void SpawnTerrain()
    {
        Debug.Log("Yay");
        
        zoom = Random.Range(0.1f, 0.3f);
        zoom1 = Random.Range(0.1f, 0.3f);
        scale = Random.Range(20f, 30f);
        
        for (int x = 0; x < 50; x++)
        {
            for (int z = 0; z < 50; z++)
            {
                cubePos.x = x;
                float perlinNoise = Mathf.PerlinNoise(x * zoom,z * zoom1);
                cubePos.y = perlinNoise * scale;
                cubePos.z = z;

                if (perlinNoise > 0.4)
                {
                    GameObject go = Instantiate(cubePrefab, cubePos, Quaternion.identity) as GameObject;
                                 
                                 if (perlinNoise > .5f)
                                 {
                                                    
                                     cubeRend = go.GetComponent<Renderer>();
                                     cubeRend.material.color = Color.red;
                 
                                 }
                }

            }
        }
       
    }
}