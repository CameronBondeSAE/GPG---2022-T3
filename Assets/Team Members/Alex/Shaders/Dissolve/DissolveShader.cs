using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer))]
public class DissolveShader : MonoBehaviour
{
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;

    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        var time = Time.time * Mathf.PI * 0.25f;

        float height = transform.localPosition.y;
        height += Mathf.Sin(time) * (objectHeight / 2.0f);
        SetHeight(height);
    }

    private void SetHeight(float height)
    {
        material.SetFloat("_CutOffHeight", height);
        material.SetFloat("_NoiseStrength", noiseStrength);
    }
}
