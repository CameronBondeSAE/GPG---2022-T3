using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Oscar
{
    public class SpawnItems : MonoBehaviour
    {
        public GameObject item;
        public void SpawnTheItems(Vector3 prefabPosition, GameObject itemParent, float perlinValue)
        {
            if (perlinValue < 0.3f)
            {
                GameObject newCube = Instantiate(item, prefabPosition, Quaternion.identity);
                newCube.transform.SetParent(itemParent.transform);
            }
        }
    }
}

