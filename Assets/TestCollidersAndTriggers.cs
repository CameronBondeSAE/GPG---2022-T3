using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestCollidersAndTriggers : MonoBehaviour
{
	public float range = 5f;
	private void Start()
	{
		for (int i = 0; i < 100; i++)
		{
			Vector3 rnd = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
			rnd.Normalize();
			
			Debug.DrawRay(rnd * 2f, Vector3.up*5f, Color.green, 10f);
		}


		for (int i = 0; i < 360; i=i+16)
		{
			Vector3 euler = Quaternion.Euler(0,i,0) * Vector3.forward;
			Debug.DrawRay(transform.position + euler, Vector3.up * 10f, Color.magenta, 10f);
			if (true)
			{
				break;
			}
		}
		
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Plant triggered! : "+other.transform.gameObject.name);
	}
}
