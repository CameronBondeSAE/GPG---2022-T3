using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneColour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
