using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEntityView : MonoBehaviour
{
    private Animator _anim;

    public float _waitTime;

    private void OnEnable()
    {
        _anim = GetComponent<Animator>();

        _anim.SetBool("IsOpen", true);
        StartCoroutine(Wait(_waitTime));
    }

    public void Death()
    {
        _anim.SetTrigger("End");
    }

    private IEnumerator Wait(float x)
    {
        yield return new WaitForSeconds(x);
        {
            Death();
        }
    }
}