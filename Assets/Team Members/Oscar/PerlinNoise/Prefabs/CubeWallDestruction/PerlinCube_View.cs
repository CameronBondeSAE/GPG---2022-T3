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
    
    public GameObject perlinWallCube;

    public ParticleSystem[] crumblingParticles;
    
    
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
        StartCoroutine(WallDestroy());
    }

    IEnumerator WallDestroy()
    {
        yield return new WaitForSeconds(1f);
        perlinCubeModel.transform.DOMoveY(-1f,3f).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(3f);
        if(NetworkManager.Singleton.IsServer) Destroy(perlinWallCube,1f);
    }
    
}
