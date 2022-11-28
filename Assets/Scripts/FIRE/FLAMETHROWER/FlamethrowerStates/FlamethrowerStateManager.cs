using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlamethrowerStateManager : MonoBehaviour
{
    public FlamethrowerModelView modelView;
    
    public MonoBehaviour currentState;
    public MonoBehaviour nextState;

    private List<MonoBehaviour> stateList = new List<MonoBehaviour>();

    public MonoBehaviour idleState;
    public MonoBehaviour shootState;
    public MonoBehaviour explodeState;
    public MonoBehaviour destroyedState;
		
    private void Start()
    {
        modelView = GetComponentInChildren<FlamethrowerModelView>();

        idleState = GetComponent<FlamethrowerIdleState>();
        shootState = GetComponent<FlamethrowerShootState>();
        explodeState = GetComponent<FlamethrowerExplodeState>();
        destroyedState = GetComponent<FlamethrowerDestroyedState>();
        
        stateList.Add(idleState);
        stateList.Add(shootState);
        stateList.Add(explodeState);
        stateList.Add(destroyedState);

        modelView.ChangeState += ChangeStateInt;
        
        ChangeStateInt(0);
    }

    private void ChangeStateInt(int x)
    {
        nextState = stateList[x];
        ChangeState(nextState);
    }

    // This works for ANY STATE
    public void ChangeState(MonoBehaviour newState)
    {
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
