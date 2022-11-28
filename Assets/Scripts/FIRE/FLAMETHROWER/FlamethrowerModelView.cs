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
    
    public event Action<int> ChangeState;

    public void OnChangeState(int x)
    {
        ChangeState?.Invoke(x);
    }

    //change current heat level
    public event Action<float> ChangeOverheat;

    public void OnChangeOverheat(float x)
    {
        ChangeOverheat?.Invoke(x);
    }
}