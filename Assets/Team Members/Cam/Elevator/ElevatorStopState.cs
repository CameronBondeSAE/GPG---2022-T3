using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStopState : MonoBehaviour
{
	// Note that even though I want sounds and fx to play when we first stop,
	// I don't do that here. The View should care about ME, not the other way round.
	// So the best way to do that is to spam out events and let them sort it out
	// ps "Action" is just a shortcut built into C# for a simple delegate definition
	public event Action ElevatorStoppedEvent;

	private void OnEnable()
	{
		ElevatorStoppedEvent?.Invoke();
	}
}
