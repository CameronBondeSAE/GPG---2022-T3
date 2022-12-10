using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExplosiveFragments : MonoBehaviour
{
	public float explosionForce;
	public Vector3 explosionEpicenter;
	public float explosionRadius;

	void Start()
    {
        Destroy(gameObject,3f);
    }

	public void Explode()
	{
		foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
		{
			rb.AddExplosionForce(explosionForce, explosionEpicenter, explosionRadius);
		}
	}
}
