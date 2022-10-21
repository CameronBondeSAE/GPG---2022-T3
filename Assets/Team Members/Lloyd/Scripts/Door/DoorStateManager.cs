using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStateManager : MonoBehaviour
{

    public MonoBehaviour startingState;
    public MonoBehaviour currentState;

    private void Start()
    {
        ChangeState(startingState);
        Debug.Log(currentState);
    }

    public void ChangeState(MonoBehaviour newState)
    {
        if (newState == currentState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.enabled = false;
        }

        newState.enabled = true;

        // New state swap over to incoming state
        currentState = newState;
    }
}