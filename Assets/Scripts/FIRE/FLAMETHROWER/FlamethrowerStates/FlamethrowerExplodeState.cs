using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Lloyd;
using Unity.Netcode;
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

    private Health health;

    public bool overheating;

    private float heatLevel;

    private float heatThreshold;

    private FlamethrowerModel throwerModel;
    
    [SerializeField] private float countDownTimer;

    private Rigidbody rb;

    private NetworkManager _nm;
    
    private void OnEnable()
    {
	    _nm = NetworkManager.Singleton;

	    if (!_nm.IsServer) return;

	    overheating = true;
        
        modelView = GetComponentInChildren<FlamethrowerModelView>();

        modelView.ChangeOverheat += ChangeOverheatLevel;

        model = GetComponent<FlamethrowerModel>();

        heatThreshold = model.overHeatPoint;

        countDownTimer = model.countDownTimer;
        
        heatThreshold = model.overHeatPoint;
        
        StartCoroutine(TickTock());
    }

    private void ChangeOverheatLevel(float amount)
    {
        heatLevel += amount;

        if (amount < heatThreshold)
            overheating = false;
        
        if(!overheating)
            modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Neutral);
    }

    private IEnumerator TickTock()
    {
        while (overheating)
        {
	        if (0 < countDownTimer)
            {
                countDownTimer --;
            }
	        else
            {
                Explode();
                overheating = false;
                yield return null;
            }
            
            yield return new WaitForSeconds(1);
        }
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

            if (burnVictims.GetComponent<Health>() != null)
            {
                health = burnVictims.GetComponent<Health>();
                health.ChangeHP(-100000);
            }
        }
        modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Explode);
    }

    private void OnDisable()
    {
	    if (!_nm.IsServer) return;
	    
        modelView.ChangeOverheat -= ChangeOverheatLevel;
        
        overheating = false;
    }
}