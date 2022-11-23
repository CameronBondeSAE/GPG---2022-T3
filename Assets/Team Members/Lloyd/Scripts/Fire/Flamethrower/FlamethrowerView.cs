using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;
public class FlamethrowerView : MonoBehaviour
{
    public FlamethrowerModelView modelView;

    private float overheatLevel;

    private void OnEnable()
    {
        modelView = GetComponentInParent<FlamethrowerModelView>();

        modelView.ChangeOverheat += ChangeOverheat;
        modelView.YouDied += Overheat;
    }

    private void ChangeOverheat(float x)
    {
        overheatLevel = x;
        Debug.Log(overheatLevel);
    }

    private void Overheat()
    {
        Debug.Log("KABOOM");
        //explode
    }
}
