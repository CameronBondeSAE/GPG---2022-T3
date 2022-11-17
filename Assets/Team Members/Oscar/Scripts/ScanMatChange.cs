using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer))]
public class ScanMatChange : MonoBehaviour
{
    private Material mat;

    public float objectHeight = 1.0f;
    public float noiseStrength = .25f;
    
    public bool SeePlayer = false;
    private float time;
    
    private float matHeight = 0;
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        SetHeight(matHeight);
    }

    private void Update()
    {
        if (SeePlayer == false)
        {
            
            time += 0.01f; //Time.time * Mathf.PI * 0.25f;

            float height = transform.localPosition.y;
            height += time * (objectHeight / 2.0f);
            SetHeight(height);
        }
        else
        {
            time = 0f;
            float height = transform.localPosition.y;
            height = -1;
            SetHeight(height);
            
            SeePlayer = false;
        }
    }

    private void SetHeight(float height)
    {
        mat.SetFloat("_CutOffHeight", height);
        mat.SetFloat("_NoiseStrength", noiseStrength);
    }
    
}
