using System.Collections.Generic;
using Luke;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    //all the spawned objects, and making them fit nicely in the hierarchy
    public GameObject cubePrefab;
    public GameObject Barrel;
    public GameObject AISpawner;
    public GameObject Base;
    
    GameObject CubeParent; 
    GameObject BarrelParent;
    GameObject AIParent;
    GameObject borderParent;
    GameObject BaseParent;
    
    //for easy delete when refreshing or resetting the map
    private List<GameObject> cubeLand = new List<GameObject>();
    private List<GameObject> BarrelList = new List<GameObject>();
    private List<GameObject> AIList = new List<GameObject>();
    private List<GameObject> BaseList = new List<GameObject>();


    //Base spawning variables
    [SerializeField] 
    private int HQAmount = 0;

    [SerializeField] 
    private float LastBaseDistance = 20;

    private Vector3 prevBasePos;
    
    [SerializeField]
    private float minDist = 20;
    

    //Perlin noise values and required elements to spawn the maze.
    public int amount;

    private float scale = 3f;
    private float zoomX;
    private float zoomZ;
    private float zoom;

    public bool randomMap;
    
    private float x;
    private float y;
    private Vector3 prefabPosition;
    public int cubeSize = 1;

    public void Start()
    {
        GameManager.Singleton.OnGameStart += Spawner;
        GameManager.Singleton.OnGameEnd += DeleteMap;
        
        CubeParent = new GameObject("CubeParent");
        BarrelParent = new GameObject("ItemParent");
        AIParent = new GameObject("AIParent");
        borderParent = new GameObject("borderParent");
        
        if (randomMap == true)
        {
            //randoms
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            //spawnTerrain(zoomX,zoomZ);
        }
        else if (randomMap == false)
        {
            //standard averages
            zoomX = 0.15f;
            zoomZ = 0.15f;
            //spawnTerrain(zoomX,zoomZ);
        }
    }

    public void ResetTheMap()
    {
        for (int cubes = 0; cubes < cubeLand.Count; cubes++)
        {
            Destroy(cubeLand[cubes].gameObject);
        }
        cubeLand.Clear();
        for (int items = 0; items < BarrelList.Count; items++)
        {
            Destroy(BarrelList[items].gameObject);
        }
        BarrelList.Clear();
        for (int aliens = 0; aliens < AIList.Count; aliens++)
        {
            Destroy(AIList[aliens].gameObject);
        }
        AIList.Clear();
        for (int bases = 0; bases < BaseList.Count; bases++)
        {
            Destroy(BaseList[bases].gameObject);
        }
        BaseList.Clear();
        Spawner();
    }

    public void Spawner()
    {
        if (randomMap == true)
        {
            zoomX = Random.Range(0.1f, 0.3f);
            zoomZ = Random.Range(0.1f, 0.3f);
            spawnTerrain(zoomX, zoomZ);
        }
        else if (randomMap == false)
        {
            spawnTerrain(zoomX, zoomZ);
        }
    }
    
    public void spawnTerrain(float zoomX, float zoomZ)
    {
        print("scale = " + scale + " zoomX = " + zoomX + " zoomZ = " + zoomZ);
        
        for (int positionX = 0; positionX < amount; positionX=positionX+cubeSize)
        { 
            for (int positionZ = 0; positionZ < amount; positionZ=positionZ+cubeSize)
            {
                float perlinValue = Mathf.PerlinNoise((positionX * zoomX), (positionZ * zoomZ));
                
                prefabPosition.x = positionX;
                prefabPosition.y = perlinValue * scale;
                prefabPosition.z = positionZ;
                
                if (perlinValue > .3)
                {
                    if (perlinValue > .5)
                    {
                        GameObject newCube = Instantiate(cubePrefab, prefabPosition, Quaternion.identity);
                        newCube.transform.SetParent(CubeParent.transform);
                        cubeLand.Add(newCube.gameObject);
                        newCube.GetComponent<Renderer>().material.color = Color.black;
                        if (perlinValue > .8)
                        {
                            newCube.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                    else
                    {
                        if (perlinValue < .4f && prefabPosition.x > 20 && prefabPosition.z > 20)
                        {
                            SpawnTheBase(prefabPosition);
                            
                        }
                        
                        SpawnAIInTheMaze(prefabPosition);
                        SpawningTheBarrels(prefabPosition);
                        //SpawningTheItems(prefabPosition);
                    }
                }
            }
        }

        SpawnWalls();
    }

    void SpawnTheBase(Vector3 prefabPosition)
    {
        //learn how to calculate distances between objects
        float tempBaseDist = Vector3.Distance(prefabPosition, prevBasePos);
        if (tempBaseDist > minDist)
        {
            if (HQAmount <= 2)
            {
                Vector3 tempBasePos = prefabPosition;
                prevBasePos = tempBasePos;
                GameObject HQ = Instantiate(Base,prefabPosition,quaternion.identity);
                HQAmount++;
                BaseList.Add(HQ);
                
            }
        }
        


    }

    void SpawningTheBarrels(Vector3 prefabPosition)
    {
        //use new perlin to create clusters of barrels
        
        int SpawmTheBarrels = Random.Range(1, 50);
        if (SpawmTheBarrels == 1)
        {
            GameObject spawnedItem = Instantiate(Barrel, prefabPosition, quaternion.identity);
            
            spawnedItem.transform.SetParent(BarrelParent.transform);
            BarrelList.Add(spawnedItem.gameObject);
            
            spawnedItem.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    void SpawnAIInTheMaze(Vector3 prefabPosition)
    {
        //create spawn location with new perlin then 
        
        int spawnTheAI = Random.Range(1, 50);
        if (spawnTheAI == 1)
        {
            GameObject spawnedAI = Instantiate(AISpawner, prefabPosition, quaternion.identity);
            
            spawnedAI.transform.SetParent(AIParent.transform);
            AIList.Add(spawnedAI.gameObject);
        }
    }
    
    void SpawnWalls()
    {
        GameObject firstWall = Instantiate(cubePrefab,
            new Vector3(0, prefabPosition.y, (amount / 2)), quaternion.identity);
        firstWall.name = "firstWall";
        firstWall.transform.localScale = new Vector3(1, prefabPosition.y * scale * 2, prefabPosition.z);
        cubeLand.Add(firstWall.gameObject);
        firstWall.GetComponent<Renderer>().material.color = Color.black;
        firstWall.transform.SetParent(borderParent.transform);

        
        GameObject secondWall = Instantiate(cubePrefab,
            new Vector3(prefabPosition.x - (amount / 2),prefabPosition.y,prefabPosition.z), Quaternion.identity);
        secondWall.name = "secondWall";
        secondWall.transform.localScale = new Vector3(prefabPosition.x,prefabPosition.y * scale * 2,1);
        cubeLand.Add(secondWall.gameObject);
        secondWall.GetComponent<Renderer>().material.color = Color.black;
        secondWall.transform.SetParent(borderParent.transform);
        
        
        GameObject thirdWall = Instantiate(cubePrefab,
            new Vector3(prefabPosition.x, prefabPosition.y, prefabPosition.z - (amount / 2)), quaternion.identity);
        thirdWall.name = "thirdWall";
        thirdWall.transform.localScale = new Vector3(1, prefabPosition.y * scale * 2, prefabPosition.z);
        cubeLand.Add(thirdWall.gameObject);
        thirdWall.GetComponent<Renderer>().material.color = Color.black;
        thirdWall.transform.SetParent(borderParent.transform);
        
        
        GameObject fourthWall = Instantiate(cubePrefab,
            new Vector3((amount / 2), prefabPosition.y, 0), quaternion.identity);
        fourthWall.name = "fourthWall";
        fourthWall.transform.localScale = new Vector3(prefabPosition.x, prefabPosition.y * scale * 2, 1);
        cubeLand.Add(fourthWall.gameObject);
        fourthWall.GetComponent<Renderer>().material.color = Color.black;
        fourthWall.transform.SetParent(borderParent.transform);
        
        
        GameObject floor = Instantiate(cubePrefab,
            new Vector3((amount / 2), 0, (amount / 2)), quaternion.identity);
        floor.name = "floor";
        floor.transform.localScale = new Vector3(amount, 1, amount);
        cubeLand.Add(floor.gameObject);
        floor.GetComponent<Renderer>().material.color = Color.green;
        floor.transform.SetParent(borderParent.transform);
    }
    
    private void DeleteMap()
    {
        for (int cubes = 0; cubes < cubeLand.Count; cubes++)
        {
            Destroy(cubeLand[cubes].gameObject);
        }
        cubeLand.Clear();
        for (int items = 0; items < BarrelList.Count; items++)
        {
            Destroy(BarrelList[items].gameObject);
        }
        BarrelList.Clear();
        for (int aliens = 0; aliens < AIList.Count; aliens++)
        {
            Destroy(AIList[aliens].gameObject);
        }
        AIList.Clear();
        for (int bases = 0; bases < BaseList.Count; bases++)
        {
            Destroy(BaseList[bases].gameObject);
        }
        BaseList.Clear();
    }
    private void SpawningTheItems(Vector3 prefabPosition)
    {
        
    }
}

    