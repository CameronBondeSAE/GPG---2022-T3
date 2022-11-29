using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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

    private bool burning;
    private Tweener curTween;

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
        curTween = body.DOColor(spreadColour, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!burning)
        {
            transform.localScale = new Vector3(plantGrowth.age, 0.05f, plantGrowth.age);
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.5f, 0.05f, 0.5f), 0.002f);
        }
    }

    void Matured(float duration)
    {
       curTween = body.DOColor(matureColour, duration);
    }

    void Dying(float duration)
    {
        curTween = body.DOColor(deathColour, duration);
    }

    void Burning()
    {
        burning = true;
        curTween.Kill();
        
        body.DOColor(burnColour, 3f);
    }
}
   