using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemAreaSpawner : MonoBehaviour
{
    public GameObject itemToSpread;

    public float itemXSpread = 10f;
    public float itemYSpread = 0f;
    public float itemZSpread = 10f;

    public int itemAmount = 4;

    public List<GameObject> itemList = new List<GameObject>();
    private GameObject itemParent;

    private void Start()
    {
        itemParent = new GameObject("ItemParent");
    }

    public void SpreadItems()
    {
        for (int items = 0; items < itemAmount; items++)
        {
            
            Vector3 randPosition = new Vector3(Random.Range(-itemXSpread, itemXSpread),
                Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread)) + transform.position;
            GameObject clone = Instantiate(itemToSpread, randPosition, quaternion.identity);
            itemList.Add(clone.gameObject);
            clone.transform.SetParent(itemParent.transform);
        }
    }

}
