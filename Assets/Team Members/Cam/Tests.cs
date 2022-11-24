using System.Collections;
using System.Collections.Generic;
using Luke;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tests : MonoBehaviour
{
    public bool hasResource;
    public string testString;

    public GameObject prefab;
    
    // Start is called before the first frame update
    void Start()
    {
	    GameManager.singleton.NetworkInstantiate(prefab, Vector3.zero, Quaternion.identity);
	    // Instantiate(prefab);
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
