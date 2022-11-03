using System.Collections;
using System.Collections.Generic;
using Luke;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;
	public Transform cameraChild;
	public float speed = 1f;
	
	// Update is called once per frame
	void Update()
	{
		if (target != null)
	    {
		    // transform.position = target.position + offset;
		    transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);

		    cameraChild.position = transform.position + offset;
		    cameraChild.LookAt(target);
	    }
    }
    
}
