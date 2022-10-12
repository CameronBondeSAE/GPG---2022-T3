using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneColour : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
