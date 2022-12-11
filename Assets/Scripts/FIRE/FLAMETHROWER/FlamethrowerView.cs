using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Lloyd;
using Unity.Netcode;

public class FlamethrowerView : NetworkBehaviour
{
    public FlamethrowerModel model;
    
    public FlamethrowerModelView modelView;
    
    private float countDownTimer;

    public GameObject flamethrower;

    public ParticleSystem Smoke;

    public GameObject smokeObj;

    public GameObject fragments;

    public enum FlamethrowerStates
    {
	    Neutral,
	    Shooting,
	    Pulsate,
	    Explode
    }
    
    private void OnEnable()
    {
        flamethrower = gameObject;
        
        model = GetComponentInParent<FlamethrowerModel>();
        countDownTimer = model.countDownTimer;
        
        modelView = GetComponentInParent<FlamethrowerModelView>();

        modelView.ChangeState += ChangeState;
    }

    // TODO: LUKE Do I need to clientRpc this?

    private void ChangeState(FlamethrowerStates flamethrowerState)
    {
	    ChangeStateClientRpc(flamethrowerState);
    }
    
    [ClientRpc]
    private void ChangeStateClientRpc(FlamethrowerStates flamethrowerState)
    {
        switch (flamethrowerState)
        {
            case FlamethrowerStates.Neutral:
                flamethrower.transform.localScale = new Vector3(1, 1, 1);
                Smoke.Stop();
                break;
            
            case FlamethrowerStates.Shooting:
                flamethrower.transform.localScale = new Vector3(1, 1, 1);
                Smoke.Stop();
                break;
            
            case FlamethrowerStates.Pulsate:
                Pulsate(countDownTimer);
                break;
            
            case FlamethrowerStates.Explode:
                Explode();
                break;
        }
    }

    private void Pulsate(float countdownTimer)
    {
        Smoke.Play();
        
        flamethrower.transform.DOShakeScale(countdownTimer,
            new Vector3(.1f,.1f,.1f), 10,5f,false);
    }

    private void Explode()
    {
	    Smoke.Stop();
        Instantiate(fragments, transform.position, Quaternion.identity);
    }
}
