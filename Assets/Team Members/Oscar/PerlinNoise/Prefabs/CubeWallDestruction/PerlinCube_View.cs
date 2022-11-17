using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine;

public class PerlinCube_View : MonoBehaviour
{
    public PerlinCube_Model perlinCubeModel;
    
    public GameObject perlinWallCube;

    public ParticleSystem[] crumblingParticles;

    private bool DestroyTheWallNow;
    
    private void OnEnable()
    {
        perlinCubeModel.wallDestruction += WallCrumble;
    }

    void WallCrumble()
    {
        perlinWallCube.GetComponent<Renderer>().material.color = Color.blue;
        perlinCubeModel.transform.DOShakeScale(3f);
        foreach (ParticleSystem debree in crumblingParticles)
        {
            debree.Play();
        }
        print("detroyed");
        DestroyTheWallNow = true;
    }

    public void FixedUpdate()
    {
        if (DestroyTheWallNow == true)
        {
	        DestroyTheWallNow = false; // CAM: Otherwise it'll retrigger many coroutine
            StartCoroutine(WallDestroy());
        }
    }

    IEnumerator WallDestroy()
    {
        yield return new WaitForSeconds(3f);
        perlinCubeModel.transform.DOScale(new Vector3(0f, 0f, 0f),3f);
        yield return new WaitForSeconds(3f);
        Destroy(perlinWallCube,1f);
    }
    
}
