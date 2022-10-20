using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_ViewModel : MonoBehaviour
{
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
