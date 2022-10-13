using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerlinScript : MonoBehaviour
{
    [Header("Terrain Object")] public GameObject cubePrefab;

    [Header("Item Prefab")] [SerializeField]
    private GameObject itemPrefab;

    private Vector3 itemPos;

    [SerializeField] private int numItems;

    

    [Header("Noise Settings")] [SerializeField]
    private int numCube;

    [SerializeField] private int cubeSize;
    [SerializeField] private float zoomX;
    [SerializeField] private float zoomY;
    [SerializeField] private float scale;

    public GameObject parentObj;

    private Vector3 cubePos;
    private Renderer cubeRend;
    private GameObject cubeSpawned;
    private bool isHighest;


    public void GenerateTerrain()
    {
        EventManager.TerrainClearFunction();
        SpawnTerrain();
    }

    void SpawnTerrain()
    {
        for (int x = 0; x < numCube; x++)
        {
            for (int z = 0; z < numCube; z++)
            {
                cubePos.x = x;
                float perlinNoise = Mathf.PerlinNoise(x * zoomX, z * zoomY);
                cubePos.y = perlinNoise * scale;
                cubePos.z = z;

                if (perlinNoise < 0.2)
                {
                    SpawnItems();
                }

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
                }
            }
        }

        PlaceGround();
    }

    void SpawnItems()
    {
        float a = 0f;
        float b = 1f;
        float perlinNoise = Mathf.PerlinNoise(a, b * scale);

        if (perlinNoise > 0.0f)
        {
            GameObject item =
                Instantiate(itemPrefab, cubePos, Quaternion.identity) as GameObject;
            item.transform.SetParent(parentObj.transform);
            cubeRend = item.GetComponent<Renderer>();
            cubeRend.material.color = Color.yellow;
        }
    }

    void PlaceWalls()
    {
    }

    void PlaceGround()
    {
        GameObject ground = Instantiate(cubePrefab, new Vector3(cubePos.x / 2, 0, cubePos.z / 2), Quaternion.identity);
        ground.transform.localScale = new Vector3(cubePos.x, cubePos.y, cubePos.z);
        cubeRend = ground.GetComponent<Renderer>();
        cubeRend.material.color = Color.blue;
    }

    public void RandomiseValues()
    {
        scale = Random.Range(5f, 8f);
        zoomX = Random.Range(.07f, .2f);
        zoomY = Random.Range(.07f, .2f);
    }
}