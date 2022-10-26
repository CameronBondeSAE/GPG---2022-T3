using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastsEtc : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		// Testing for SOMETHING in an area
		Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

		foreach (Collider item in colliders)
		{
			//item.// Do stuff
		}
		
		// Raycasts and Linecasts
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hitInfo = new RaycastHit();
		Physics.Raycast(ray, out hitInfo, 999f);
 		
		Debug.DrawLine(hitInfo.point, hitInfo.point+hitInfo.normal, Color.green);

		// The default behaviour of variables is to COPY the contents to a function
		int myVar = 666;
		DoThing(myVar);
		Debug.Log(myVar);
	}

	private void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contactPoint in collision.contacts)
		{
			//contactPoint. // Do stuff
		}
	}

	public void DoThing(int stuff)
	{
		stuff = 9999;
	}
}