using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Lloyd;
using Unity.VisualScripting;
using UnityEngine.XR;

public class FlamethrowerExplodeState : MonoBehaviour, IHeatSource
{
    private Flammable flammable;
    
    [Header("EXPLODE POWER")] [SerializeField]
    private float explodePower;

    [Header("EXPLODE FIRE DAMAGE")] [SerializeField]
    private float fireDamage;

    [Header("How powerful the upwards force of the explosion is")] [SerializeField]
    private float explodeUpPower;

    [Header("How large the explode circle is")] [SerializeField]
    private float radius;

    private FlamethrowerModel model;

    private FlamethrowerModelView modelView;

    public bool overheating;

    private float heatLevel;

    private float heatThreshold;

    private FlamethrowerModel throwerModel;
    
    [SerializeField] private float countDownTimer;

    private Rigidbody rb;
    
    private void OnEnable()
    {
        modelView = GetComponentInChildren<FlamethrowerModelView>();

        model = GetComponent<FlamethrowerModel>();

        countDownTimer = model.countDownTimer;

        heatLevel = model.overHeatLevel;

        heatThreshold = model.overHeatPoint;
        
        StartCoroutine(TickTock());
    }

    private IEnumerator TickTock()
    {
        overheating = model.overheating;
        while (overheating)
        {
            int timesup = 0;
            if (timesup < countDownTimer)
            {
                countDownTimer --;
            }

            yield return new WaitForSeconds(countDownTimer);

            if (countDownTimer == timesup)
            {
                Explode();
                yield return null;
            }
        }
        if(!overheating)
            ChangeBack();
    }

    private void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider burnVictims in colliders)
        {
            if (burnVictims.GetComponent<Flammable>() != null)
            {
                flammable = burnVictims.GetComponent<Flammable>();
                flammable.ChangeHeat(this, fireDamage);
            }

            Rigidbody rb = burnVictims.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explodePower, explosionPos, radius, explodeUpPower);
        }
        this.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.mass = 20;
        rb.AddForce(new Vector3(0, explodePower, 0));

        modelView.OnChangeState(3);
    }
    
    private void ChangeBack()
    {
        modelView.OnChangeState(0);
    }

    private void OnDisable()
    {
        
    }
}