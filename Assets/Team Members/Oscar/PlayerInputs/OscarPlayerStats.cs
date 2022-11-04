using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscarPlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().drag = 0.5f;
    }
}
