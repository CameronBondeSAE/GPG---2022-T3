using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using UnityEngine;

[Serializable]
public class GuyBro_Model : MonoBehaviour, IFlammable
{
	public int broness;
	public string nickName;
	public float floatNonAir;
	// public GameObject myFam;
	
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
