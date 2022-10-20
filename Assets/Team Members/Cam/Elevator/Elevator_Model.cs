using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Model : MonoBehaviour
{
	public MonoBehaviour startingState;
	
	private void Start()
	{
		
	}

	
	
	
	
	
	
	
	
	
	

	// States
	// Up
	// Down
	// GoingUp
	// GoingDown
	// Stopped
	
	// GameObject wide variables/functions
	private bool occupied;
	public float speed;
	private bool moving;
	private int level;
	private int destination;
	
	public void OpenDoor(){}
	public void CloseDoor(){}

	// State specific variables
	
	// State specific functions


	public enum ElevatorState
	{
		stopped,
		movingUp,
		movingDown
	}

	public ElevatorState state;
	public ElevatorState lastState;

	private void OnTriggerEnter(Collider other)
	{
		state = ElevatorState.movingUp;
	}

	public void Update()
	{
		// switch (state)
		// {
		// 	case ElevatorState.stopped:
		// 		break;
		// 	case ElevatorState.movingUp:
		// 		if (lastState != state)
		// 		{
		// 			if (lastState == ElevatorState.stopped)
		// 			{
		// 				// playstartsound
		// 				lastState = state;
		// 			}
		// 		}		
		// 		transform.Translate(0,speed * Time.deltaTime,0);
		// 		// if(y>100)
		// 		//  playSound
		// 		//	state = stopped;
		// 		break;
		// 	case ElevatorState.movingDown:
		// 		transform.Translate(0,-speed * Time.deltaTime,0);
		// 		break;
		// 	default:
		// 		throw new ArgumentOutOfRangeException();
		// }
		//
		
	}
}
