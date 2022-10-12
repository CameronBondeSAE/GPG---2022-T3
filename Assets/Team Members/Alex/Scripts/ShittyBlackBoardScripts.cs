using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class ShittyBlackBoardScripts : MonoBehaviour
{
    
    
    public Blackboard blackboard;

    public bool movingToResource;

    public bool gatheringResource;

    public bool hasResource;

    public bool seeResource;
    public bool atResource;
    public bool atBase;
    public bool gatherResource;
    public bool findResorce;

    // Start is called before the first frame update
    void Start()
    {
        seeResource = false;
        movingToResource = false;
        gatheringResource = false;
        hasResource = false;
        seeResource = false;
        atResource = false;
        atBase = false;
        gatherResource = true;
        findResorce = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
