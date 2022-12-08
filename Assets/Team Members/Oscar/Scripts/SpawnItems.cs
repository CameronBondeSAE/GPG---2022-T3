using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Oscar
{
    public class SpawnItems : MonoBehaviour
    {
        public GameObject item;
        public void SpawnTheItems(List<Vector3> prefabPosition)
        {
            for (int i = 0; i < prefabPosition.Count; i++)
            {
                GameObject newCube = Instantiate(item, new Vector3(prefabPosition[i].x,1f,prefabPosition[i].z), Quaternion.identity);
                //newCube.transform.SetParent(itemParent.transform);
            }
            
        }
    }
}

