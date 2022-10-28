using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballView : MonoBehaviour
{
    private Animator _anim;

    private void OnEnable()
    {
        _anim = GetComponent<Animator>();

        StartCoroutine(Wait(.8f));
    }
    
    private void Death()
    {
        _anim.SetBool("IsOpen", false);
        _anim.SetTrigger("End");

        StartCoroutine(Wait(1.2f));
    }

    private void OnDisable()
    {
        
    }

    private IEnumerator Wait(float x)
    {
        yield return new WaitForSeconds(x);
        {
            _anim.SetBool("IsOpen", true);
        }
    }
}