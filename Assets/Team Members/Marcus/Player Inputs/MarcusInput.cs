using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcusInput : MonoBehaviour
{
    public Material[] playerColour;
    private MeshRenderer mr;
    
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.material = playerColour[Random.Range(0, playerColour.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
