using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorView : MonoBehaviour
{
    // // // //
    // DOOR COMPONENTS
    private DoorComponents _doorComp;

    private int _mainDoorInt;

    private int _doorInt;

    private float _HP;

    private float _fireDamage;

    private float _speed;

    private Renderer _rend;

    private void OnEnable()
    {
        _doorComp = GetComponent<DoorComponents>();
        GetComps();

        _rend = GetComponent<Renderer>();

        _rend.material.SetColor("_BaseColor", new Color(0.3f, 0.4f, 0.6f, 0.3f));

        Lloyd.EventManager.ChangeHealthEvent += ChangeHP;
        Lloyd.EventManager.BurningEvent += Burning;
        Lloyd.EventManager.BurntEvent += Burnt;
    }

    private void GetComps()
    {
        if (_doorComp != null)
        {
            _doorComp.GetDoorComps(out int a, out int b, out float c, out float d, out float e);
            _mainDoorInt = a;
            _doorInt = b;
            _HP = c;
            _fireDamage = d;
            _speed = e;
        }
    }

    public void ChangeHP(float amount)
    {
        _HP += amount;
    }

    private void FixedUpdate()
    {
        // doorText.text = HP.ToString(); 
    }

    public void Burning()
    {
        _rend.material.SetColor("_BaseColor", Color.red);
    }

    public void Burnt()
    {
        _rend.material.SetColor("_BaseColor", Color.black);
    }

    private void OnDisable()
    {
        Lloyd.EventManager.ChangeHealthEvent -= ChangeHP;
        Lloyd.EventManager.BurningEvent -= Burning;
        Lloyd.EventManager.BurntEvent -= Burnt;
    }
}