using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineStopTest : MonoBehaviour
{
	public Coroutine co;
	
	// Start is called before the first frame update
    void Update()
    {
	    if (co != null)
	    {
		    StopCoroutine(co);
	    }
	    co = StartCoroutine(Thing());
    }

    private IEnumerator Thing()
    {
	    Debug.Log("Before");
	    yield return new WaitForSeconds(4);
	    Debug.Log("After");
    }
}
