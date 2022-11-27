using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Lloyd;
public class FlamethrowerView : MonoBehaviour
{
    public FlamethrowerModelView modelView;

    private float overheatLevel;

    public GameObject flamethrower;

    public ParticleSystem Smoke;

    public GameObject fragments;

    private void OnEnable()
    {
        modelView = GetComponentInParent<FlamethrowerModelView>();

        modelView.ChangeOverheat += ChangeOverheat;

        modelView.Pulsing += Pulsate;
        
        modelView.Explode += Explode;
    }

    private void ChangeOverheat(float x)
    {
        overheatLevel = x;
        //Debug.Log(overheatLevel);
    }

    private void Pulsate()
    {
        Smoke.Play();
        
        flamethrower.transform.DOShakeScale(3f,
            new Vector3(.1f,.1f,.1f), 5,5f,false);
    }

    private void Explode()
    {
        //Debug.Log("KABOOM");
        //explode
    }
}
