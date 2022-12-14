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
    Transform t;

    private bool isVisable;

    private void Awake()
    {
        t = transform;
    }

    private void OnEnable()
    {
	    plantGrowth.GrowEvent += Growing;
	    plantMature.MatureEvent += Matured;
	    plantDeath.DeathEvent += Dying;
	    plantBase.BurningEvent += Burning;
    }
    private void OnDisable()
    {
	    plantGrowth.GrowEvent -= Growing;
	    plantMature.MatureEvent -= Matured;
	    plantDeath.DeathEvent -= Dying;
	    plantBase.BurningEvent -= Burning;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float plantAge = (plantGrowth.age / 2) + 0.5f;
        
        if (isVisable)
        {
            if (!burning)
            {
                t.localScale = new Vector3(plantAge, 0.05f, plantAge);
            }
            else
            {
                t.localScale = Vector3.MoveTowards(t.localScale, new Vector3(plantAge/2f, 0.05f, plantAge/2f), 0.002f);
            }
        }
    }

    void Growing()
    {
        // curTween.Kill();
        // curTween = body.DOColor(spreadColour, 5f);
    }
    
    void Matured(float duration)
    {
        // curTween = body.DOColor(matureColour, duration);
    }

    void Dying(float duration)
    {
        // curTween = body.DOColor(deathColour, duration);
    }

    void Burning()
    {
        burning = true;
        // curTween.Kill();
        
        // curTween = body.DOColor(burnColour, 3f);
    }

    private void OnBecameVisible()
    {
        isVisable = true;
    }

    private void OnBecameInvisible()
    {
        isVisable = false;
    }
}
   