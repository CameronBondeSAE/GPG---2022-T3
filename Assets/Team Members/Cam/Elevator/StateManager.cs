using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
	public class StateManager : MonoBehaviour
	{
		public MonoBehaviour startingState;
		public MonoBehaviour currentState;
		
		private void Start()
		{
			// ChangeState(startingState);
		}

		// This works for ANY STATE
		public void ChangeState(StateBase newState)
		{
			// Check if the state is the same and DON'T swap
			if (newState == currentState)
			{
				return;
			}

			// At first 'currentstate' will ALWAYS be null
			if (currentState != null)
			{
				currentState.enabled = false;
			}

			newState.enabled = true;

			// New state swap over to incoming state
			currentState = newState;
		}
	}
}
