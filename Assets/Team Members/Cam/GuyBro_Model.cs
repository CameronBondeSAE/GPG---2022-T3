using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyBro_Model : MonoBehaviour, IFlammable
{
	public void SetOnFire()
	{
		Debug.Log("FEEL THE BURN!!! OH YEAH!");
	}

	private void OnCollisionEnter(Collision collision)
	{
		// collision.body.GetComponent<>()
	}

	private void OnTriggerEnter(Collider other)
	{
		// other.
	}
}
