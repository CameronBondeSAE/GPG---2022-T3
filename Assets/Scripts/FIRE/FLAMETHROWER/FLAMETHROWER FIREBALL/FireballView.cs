using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballView : MonoBehaviour
{
    private Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }

    public void Death()
    {
        anim.SetTrigger("End");
    }
}