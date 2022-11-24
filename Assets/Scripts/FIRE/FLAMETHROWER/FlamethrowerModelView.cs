using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerModelView : MonoBehaviour
{
    public event Action<float> ChangeOverheat;

    public void OnChangeOverheat(float x)
    {
        ChangeOverheat?.Invoke(x);
    }
    
    public event Action Pulsing;

    public void OnPulsing()
    {
        Pulsing?.Invoke();
    }

    public event Action YouDied;

    public void OnYouDied()
    {
        YouDied?.Invoke();
    }
}
