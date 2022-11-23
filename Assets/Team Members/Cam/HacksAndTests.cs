using System.Collections;
using System.Collections.Generic;
using Alex;
using Lloyd;
using UnityEngine;

public class HacksAndTests : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
	    yield return new WaitForSeconds(0.5f);
	    foreach (HQ hq in FindObjectsOfType<HQ>())
	    {
		    hq.DestroyLand(12f);
	    }
	    FindObjectOfType<MarcusTerrain>().Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
