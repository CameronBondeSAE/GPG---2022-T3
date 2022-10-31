using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SludgeIdleState : MonoBehaviour, IFlammable
{
    //needs to be effected by fire as fire is one of the only weapons.
    public void SetOnFire()
    {
        GetComponent<Oscar.StateManager>().ChangeState(GetComponent<SludgeNearlyExplodeState>());
    }

    private void OnDisable()
    {
        
    }
}
