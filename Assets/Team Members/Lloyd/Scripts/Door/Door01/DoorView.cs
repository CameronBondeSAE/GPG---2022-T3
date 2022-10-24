using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorView : MonoBehaviour
{
    private float HP;

    private Renderer _rend;

    private void OnEnable()
    {

        _rend = this.GetComponent<Renderer>();
        
        _rend.material.SetColor("_BaseColor", new Color(0.3f, 0.4f, 0.6f, 0.3f));

        Lloyd.EventManager.ChangeHealthEvent += ChangeHP;
        Lloyd.EventManager.BurningEvent += Burning;
        Lloyd.EventManager.BurntEvent += Burnt;
    }

    public void ChangeHP(float amount)
    { 
        HP += amount;
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
