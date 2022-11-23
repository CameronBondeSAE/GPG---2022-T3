using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oscar
{
    public class SpawnEnvironment : MonoBehaviour
    {
        public GameObject borderPrefab;
        public GameObject floorPrefab;
        public GameObject perlinPrefab;

        private List<Vector3> prefabPosition;
        private GameObject CubeParent;
        private float perlinValue;
        
        public void SpawnPerlinWalls(List<Vector3> prefabPosition, GameObject CubeParent, float perlinValue)
        {
            for (int i = 0; i < prefabPosition.Count; i++)
            {
                GameObject newCube = Instantiate(perlinPrefab, prefabPosition[i], Quaternion.identity);
                newCube.transform.SetParent(CubeParent.transform);
                newCube.GetComponent<Renderer>().material.color = Color.black;
            }
        }
        
        public void SpawnTheEnvironment(Vector3 prefabPosition, int amount, float scale, GameObject borderParent)
        {
            //spawn all the walls
            GameObject firstWall = Instantiate(borderPrefab,
                new Vector3(-1, prefabPosition.y, (amount / 2)), Quaternion.identity);
            firstWall.name = "firstWall";
            firstWall.transform.localScale = new Vector3(1, prefabPosition.y * scale * 2, prefabPosition.z + 2);
            firstWall.transform.SetParent(borderParent.transform);

            GameObject secondWall = Instantiate(borderPrefab,
                new Vector3(prefabPosition.x - (amount / 2),prefabPosition.y,prefabPosition.z + 1f), Quaternion.identity);
            secondWall.name = "secondWall";
            secondWall.transform.localScale = new Vector3(prefabPosition.x + 2,prefabPosition.y * scale * 2,1);
            secondWall.transform.SetParent(borderParent.transform);

            GameObject thirdWall = Instantiate(borderPrefab,
                new Vector3(prefabPosition.x + 1f, prefabPosition.y, prefabPosition.z - (amount / 2)), Quaternion.identity);
            thirdWall.name = "thirdWall";
            thirdWall.transform.localScale = new Vector3(1, prefabPosition.y * scale * 2, prefabPosition.z + 2);
            thirdWall.transform.SetParent(borderParent.transform);
            
            GameObject fourthWall = Instantiate(borderPrefab,
                new Vector3((amount / 2), prefabPosition.y, -1), Quaternion.identity);
            fourthWall.name = "fourthWall";
            fourthWall.transform.localScale = new Vector3(prefabPosition.x + 2, prefabPosition.y * scale * 2, 1);
            fourthWall.transform.SetParent(borderParent.transform);

            //spawn the floor
            GameObject floor = Instantiate(floorPrefab,
                new Vector3((amount / 2), 0, (amount / 2)), Quaternion.identity);
            floor.name = "floor";
            floor.transform.localScale = new Vector3(amount, 1, amount);
            floor.transform.SetParent(borderParent.transform);
        }
    }

}
