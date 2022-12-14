using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballView : MonoBehaviour
{
   // private Animator anim;

    private void OnEnable()
    {
	    GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(1f, 0, 0, .5f));;
	    //anim = GetComponent<Animator>();
    }

    public void Death()
    {
        //anim.SetTrigger("End");
    }
}