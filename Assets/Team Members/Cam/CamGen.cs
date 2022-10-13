using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGen : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	GameObject genParent;

	public bool IsThing = false;
	
    public void Generate()
    {
	    if (IsThing)
	    {
		    Debug.Log("IsThing");
	    }
	    
	    
	    if (genParent != null)  // Destroy anything from a previous run
	    {
		    DestroyImmediate(genParent);
		}

	    genParent = new GameObject("CamGenParent");

	    GameObject newGo;
	    for (int i = 0; i < 100; i++)
	    {
		    newGo = Instantiate(prefab);
		    newGo.transform.SetParent(genParent.transform);
	    }
    }
}
