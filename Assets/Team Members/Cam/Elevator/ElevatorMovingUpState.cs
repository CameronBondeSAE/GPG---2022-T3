using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovingUpState : MonoBehaviour
{
	public float speed;
	public float maxHeight = 10f;
	
	private void OnEnable()
	{
		
	}

	private void Update()
    {
	    transform.Translate(0,speed * Time.deltaTime,0);

	    if (transform.position.y > maxHeight)
	    {
		    // End somehow
		    // HACK: Shouldn't really be the responibility of this state to know about other states
		    // GetComponent<Cam.StateManager>().ChangeState(GetComponent<ElevatorStopState>());
	    }
    }

	private void OnDisable()
	{
		
	}
}
