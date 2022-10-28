using System;
using System.Collections;
using System.Collections.Generic;
using Oscar;
using UnityEngine;

public class ExplosiveSmoke : MonoBehaviour
{
    public ParticleSystem Smoke;
    public ExplosiveNearlyExplodeState AlmostBlowUp;
    private void OnEnable()
    {
        AlmostBlowUp.AlmostExplode += SmokeDispense;
    }

    public void SmokeDispense()
    {
        Smoke.Play();
    }

}
