using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoroutineStopTest : MonoBehaviour
{
	public bool beingWorthwhile = true;
	public bool ohNo = false;
	public Func<bool> aFunctionAddress;
	private Action thing;
	private Action<int> thing2;
	Coroutine coroutine;
	
	private void Start()
	{
		if (coroutine != null) StopCoroutine(coroutine);
		coroutine = StartCoroutine(CamsDoingSomething());
	}

	private void Update()
	{
		if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
		{
			StopCoroutine(coroutine);
			Debug.Log("KING HIT");
		}
	}

	private IEnumerator CamsDoingSomething()
	{
		while (beingWorthwhile)
		{
			Debug.Log("I'm worthwhile!");
			yield return new WaitForSeconds(1f);
			Debug.Log("It's going great!");
			Coroutine co = StartCoroutine(Thing());
			yield return new WaitUntil(() => co == null);
			yield return new WaitUntil(() => ohNo);
			Debug.Log("What's that smoke?");
			yield return new WaitForSeconds(3f);
			yield return new WaitForEndOfFrame();
			Debug.Log("OH... OH NO.... I DID TOO MUCH");
		}

		yield return null;
	}


	// public Coroutine co;
	//
	// // Start is called before the first frame update
 //    void Update()
 //    {
	//     if (co != null)
	//     {
	// 	    StopCoroutine(co);
	//     }
	//     co = StartCoroutine(Thing());
 //    }
 //
 private IEnumerator Thing()
 {
	 Debug.Log("Before");
	 yield return new WaitForSeconds(4);
	 Debug.Log("After");
	 yield return null;
 }
}
