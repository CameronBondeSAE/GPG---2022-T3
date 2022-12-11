using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class FlamethrowerStateManager : NetworkBehaviour
{
    public FlamethrowerModelView modelView;
    
    public MonoBehaviour currentState;
    public MonoBehaviour nextState;

    private List<MonoBehaviour> stateList = new List<MonoBehaviour>();

    public MonoBehaviour idleState;
    public MonoBehaviour shootState;
    public MonoBehaviour explodeState;
    public MonoBehaviour destroyedState;

    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();
	    
	    if(!IsServer) return;
	    
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
	    
	    ChangeStateInt(FlamethrowerView.FlamethrowerStates.Neutral);
    }

    private void ChangeStateInt(FlamethrowerView.FlamethrowerStates flamethrowerState)
    {
        nextState = stateList[(int)flamethrowerState];
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
