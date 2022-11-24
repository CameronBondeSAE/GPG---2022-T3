using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEntityView : MonoBehaviour
{
    private Animator _anim;

    private SpriteRenderer _spr;

    public float _waitTime;

    private void OnEnable()
    {
        _anim = GetComponent<Animator>();

        _anim.SetBool("IsOpen", true);
        StartCoroutine(Wait(_waitTime));
    }

    private IEnumerator Death()
    {
        _anim.SetTrigger("Death");
        yield return new WaitForSeconds(.75f);
        Destroy(this.gameObject);
    }

    private IEnumerator Wait(float x)
    {
        yield return new WaitForSeconds(_waitTime);
        {
            StartCoroutine(Death());
        }
    }
}