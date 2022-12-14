using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameModelView : MonoBehaviour
{
    //
    public event Action<bool> ChangeVisibility;

    public void OnChangeVisibility(bool x)
    {
        ChangeVisibility?.Invoke(x);
    }
}
