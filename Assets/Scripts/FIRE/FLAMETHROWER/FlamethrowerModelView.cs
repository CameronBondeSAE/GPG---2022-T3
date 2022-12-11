using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerModelView : MonoBehaviour
{
    //change state by int
    
    //idle = 0
    //idleheld = 1
    //shoot = 2
    //pulsating = 3
    //exploded = 4
    
    public event Action<FlamethrowerView.FlamethrowerStates> ChangeState;

    public void OnChangeState(FlamethrowerView.FlamethrowerStates flamethrowerState)
    {
        ChangeState?.Invoke(flamethrowerState);
    }

    //change current heat level
    public event Action<float> ChangeOverheat;

    public void OnChangeOverheat(float x)
    {
        ChangeOverheat?.Invoke(x);
    }
}