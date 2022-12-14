using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTester : MonoBehaviour
{
	private void OnBecameVisible()
	{
		Debug.Log("HI!");
		transform.localScale = Vector3.one * 2f;
	}

	private void OnBecameInvisible()
	{
		Debug.Log("Bye!");
		
		transform.localScale = Vector3.one;
	}
}
