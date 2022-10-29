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

    public void Death()
    {
         _anim.SetBool("IsOpen", true);
                _anim.SetTrigger("End");
    }

    private IEnumerator Wait(float x)
    {
        yield return new WaitForSeconds(x);
        {
            _anim.SetBool("IsOpen", true);
        }
    }
}