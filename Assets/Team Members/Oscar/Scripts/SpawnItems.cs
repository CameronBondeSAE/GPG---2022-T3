using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Oscar
{
    public class SpawnItems : MonoBehaviour
    {
        public GameObject item;
        public void SpawnTheItems(List<Vector3> prefabPosition, GameObject itemParent, float perlinValue)
        {
            for (int i = 0; i < prefabPosition.Count; i++)
            {
                if (perlinValue < 0.3f)
                {
                    GameObject newCube = Instantiate(item, prefabPosition[i], Quaternion.identity);
                    newCube.transform.SetParent(itemParent.transform);
                }
            }
            
        }
    }
}

