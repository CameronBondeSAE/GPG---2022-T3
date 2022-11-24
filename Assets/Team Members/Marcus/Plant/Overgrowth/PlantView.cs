using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Marcus;
using UnityEngine;

public class PlantView : MonoBehaviour
{
    public GrowthState plantGrowth;
    public MatureState plantMature;
    public DyingState plantDeath;
    public PlantBase plantBase;
    
    private Material body;
    private Color spreadColour = new Color32(0, 180, 19, 255);
    private Color matureColour = new Color32(0, 106, 0, 255);
    private Color deathColour = new Color32(135, 106, 0, 255);
    private Color burnColour = Color.black;

    private void OnEnable()
    {
        plantMature.MatureEvent += Matured;
        plantDeath.DeathEvent += Dying;
        plantBase.BurningEvent += Burning;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Renderer>().material;
        body.DOColor(spreadColour, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(plantGrowth.age, 0.05f, plantGrowth.age);
    }

    void Matured(float duration)
    {
       body.DOColor(matureColour, duration);
    }

    void Dying(float duration)
    {
       body.DOColor(deathColour, duration);
    }

    void Burning()
    {
        //Tween towards black and shrink
        body.DOColor(burnColour, 3f);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one / 2, 3f);
    }
}
