using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Oscar;

[RequireComponent(typeof(Renderer))]
public class ScanMatChange : MonoBehaviour, IAffectedByRadar
{
    private Material mat;

    public float noiseStrength = .25f;
    
    private float time;
    
    private float matHeight;
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        matHeight = transform.localPosition.y;
        matHeight = 0f;
        SetHeight(matHeight);
    }
    
    public void Detection()
    {
        print("changed colour");
        print("changed back");
    }
    
    private void SetHeight(float height)
    {
        mat.SetFloat("_CutOffHeight", height);
        mat.SetFloat("_NoiseStrength", noiseStrength);
    }
}
