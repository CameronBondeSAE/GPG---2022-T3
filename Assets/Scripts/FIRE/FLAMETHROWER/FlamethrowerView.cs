using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Lloyd;
public class FlamethrowerView : MonoBehaviour
{
    public FlamethrowerModel model;
    
    public FlamethrowerModelView modelView;
    
    private float countDownTimer;

    public GameObject flamethrower;

    public ParticleSystem Smoke;

    public GameObject smokeObj;

    public GameObject fragments;

    private void OnEnable()
    {
        flamethrower = gameObject;
        
        model = GetComponentInParent<FlamethrowerModel>();
        countDownTimer = model.countDownTimer;
        
        modelView = GetComponentInParent<FlamethrowerModelView>();

        modelView.ChangeState += ChangeState;
    }

    private void ChangeState(int x)
    {
        switch (x)
        {
            case 0:

                break;
            
            case 1:

                break;
            
            case 2:
                Pulsate(countDownTimer);
                break;
            
            case 3:
                Explode();
                break;
        }
    }

    private void Pulsate(float x)
    {
        Smoke.Play();
        
        flamethrower.transform.DOShakeScale(x,
            new Vector3(.1f,.1f,.1f), 10,5f,false);
    }

    private void Explode()
    {
            Smoke.Stop();
            Instantiate(fragments, transform.position, Quaternion.identity);
    }
}
