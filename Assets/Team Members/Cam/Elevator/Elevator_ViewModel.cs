using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_ViewModel : MonoBehaviour
{
	// ViewModel script are simply middlemen to help communication between the Model/Logic code,
	// and the View which is just for superficial stuff like graphics, fx, sound etc
	// Just subscribe to events coming from model code and hack away.
	public ElevatorStopState elevatorStopState;
	public AudioSource audioSource;
	public AudioClip stoppedClip;
	
	private void OnEnable()
	{
		elevatorStopState.ElevatorStoppedEvent += ElevatorStoppedEvent;
	}

	private void OnDisable()
	{
		elevatorStopState.ElevatorStoppedEvent -= ElevatorStoppedEvent;
	}

	private void ElevatorStoppedEvent()
	{
		audioSource.clip = stoppedClip;
		audioSource.Play();
	}

}
