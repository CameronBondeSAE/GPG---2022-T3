using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SludgeNearlyExplodeState : MonoBehaviour
{
    private Color purple = new Color32(143,0,254,1);

    public event Action SludgeExplode; 
    IEnumerator Start()
    {
        //make smoke from fire
        SludgeExplode?.Invoke();
        //play the SFX

        //pulse its size before exploding
        transform.DOShakeScale(3f,
            new Vector3(.1f,.1f,.1f), 5,5f,false);
        
        //change colour to red gradually
        GetComponent<Renderer>().material.DOColor(purple, 2f);
        
        yield return new WaitForSeconds(3);
        GetComponent<Oscar.StateManager>().ChangeState(GetComponent<SludgeExplodeState>());
    }

    private void OnDisable()
    {
        
    }
}
