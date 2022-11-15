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
    
    private void OnEnable()
    {
        perlinCubeModel.wallDestruction += WallDecay;
    }

    void WallDecay()
    {
        perlinCubeModel.transform.DOShakeScale(3f);
        foreach (ParticleSystem debree in crumblingParticles)
        {
            debree.Play();
        }
        //perlinCubeModel.transform.DOScale(new Vector3(.1f, .1f, .1f), 3f);
        print("detroyed");
    }
}
