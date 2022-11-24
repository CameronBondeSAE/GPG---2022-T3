using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tests : MonoBehaviour
{
    public bool hasResource;
    public string testString;
    
    // Start is called before the first frame update
    void Start()
    {
        // blackboard.SetVariableValue("testString", "Cam woz ere");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(testString);
    }

    public void ChangeState(string stateToChangeTo)
    {
        Debug.Log("Test function from BT : "+stateToChangeTo);
    }
}
