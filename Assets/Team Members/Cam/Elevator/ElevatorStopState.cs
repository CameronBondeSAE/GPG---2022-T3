using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStopState : MonoBehaviour
{
	public event Action ElevatorStoppedEvent;

	private void OnEnable()
	{
		ElevatorStoppedEvent?.Invoke();
	}
}
