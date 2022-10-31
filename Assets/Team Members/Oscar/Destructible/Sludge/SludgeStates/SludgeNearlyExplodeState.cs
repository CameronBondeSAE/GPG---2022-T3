using System;
using System.Collections;
using System.Collections.Generic;
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

        GetComponent<Renderer>().material.color = purple;
        yield return new WaitForSeconds(3);
        GetComponent<Oscar.StateManager>().ChangeState(GetComponent<SludgeExplodeState>());

    }

    private void OnDisable()
    {
        
    }
}
