using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class PerlinCube_View : MonoBehaviour
{
    public PerlinCube_Model perlinCubeModel;
    
    public ParticleSystem[] crumblingParticles;

    public float firstDelay = 1;
    public float secondDelay = 3;

    public bool visible;
    
    private void OnBecameVisible()
    {
        //print("visible");
        visible = true;
        //transform.localScale = new Vector3(1f,2.5f,1f);
    }
    
    private void OnBecameInvisible()
    {
        //print("gone");
        visible = false;
        //transform.localScale = new Vector3(.5f,.5f,.5f);
    }
    private void OnEnable()
    {
        perlinCubeModel.wallDestruction += WallCrumble;
    }

    private void OnDisable()
    {
        perlinCubeModel.wallDestruction += WallCrumble;
    }

    void WallCrumble()
    {
        if (visible)
        {
            GetComponent<Renderer>().material.color = Color.blue;
            transform.DOShakeScale(3f);
            foreach (ParticleSystem debree in crumblingParticles)
            {
                debree.Play();
            }
            StartCoroutine(WallDestroy());
        }
        else
        {
            StartCoroutine(WallDestroy());
        }
        
    }

    IEnumerator WallDestroy()
    {
        yield return new WaitForSeconds(firstDelay);
        if (visible)
        {
            transform.DOMoveY(-1f,3f).SetEase(Ease.InOutBack);
        }
        yield return new WaitForSeconds(secondDelay);
    }
    
}
