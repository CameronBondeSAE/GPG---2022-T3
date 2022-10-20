using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingVarsToFile : MonoBehaviour
{
	public GuyBroProfile guyBroProfile;
	
    // Start is called before the first frame update
    IEnumerator Start()
    {
	    // Saving
	    string json = JsonUtility.ToJson(guyBroProfile);
	    Debug.Log(json);

	    // Loading
	    yield return new WaitForSeconds(5f);
	    JsonUtility.FromJsonOverwrite(json, guyBroProfile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
