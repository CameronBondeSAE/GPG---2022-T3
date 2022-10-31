using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooSmoke : MonoBehaviour
{
    public ParticleSystem Smoke;
    public SludgeNearlyExplodeState AlmostGooExplode;
    private void OnEnable()
    {
        AlmostGooExplode.SludgeExplode += SmokeDispense;
    }
    
    public void SmokeDispense()
    {
        Smoke.Play();
    }

}