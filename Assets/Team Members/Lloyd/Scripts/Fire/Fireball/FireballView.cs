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

        _anim.SetBool("IsOpen", true);
    }

    public void Death()
    {
                _anim.SetTrigger("End");
    }
}