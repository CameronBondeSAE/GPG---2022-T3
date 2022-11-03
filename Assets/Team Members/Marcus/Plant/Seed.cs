using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject plant;
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(plant, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
