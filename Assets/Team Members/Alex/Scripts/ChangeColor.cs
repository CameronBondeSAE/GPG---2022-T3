using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChangeColor : MonoBehaviour
{
    public Flammable flammable;

    public Material nMaterial;
    public Shader defaultShader;
    public Shader agroShader;
    public Shader lookingForResourceShader;

    public Material defaultMaterial;
    public Material agroMaterial;
    public Material lookingForResourceMaterial;

    public Color defaultColor;
    public Color agroColor;
    public float colorChangeTime = 3f;

    private void Awake()
    {
        nMaterial.color = defaultColor;
        agroMaterial.color = agroColor;
        defaultMaterial.color = defaultColor;
    }

    private void FixedUpdate()
    {
        if (flammable.HeatLevel <= 1)
        {
            setColorToDefault();
        }

        if (flammable.HeatLevel > 1 && flammable.HeatLevel <= 30)
        {
            setColorToRed();
        }

        if (flammable.HeatLevel > 31 && flammable.HeatLevel <= 60)
        {
            setColorToYellow();
        }
        if (flammable.HeatLevel > 61 && flammable.HeatLevel <= 100)
        {
            setColorToWhite();
        }
    }

    public void setColorToDefault()
    {
        nMaterial.DOColor(defaultColor, colorChangeTime);
        agroMaterial.DOColor(agroColor, colorChangeTime);
        defaultMaterial.DOColor(defaultColor, colorChangeTime);
    }
    

    public void setColorToRed()
    {
        //nMaterial.SetColor("_Color", Color.red);
        nMaterial.DOColor(Color.red, colorChangeTime);
        agroMaterial.DOColor(Color.red, colorChangeTime);
        defaultMaterial.DOColor(Color.red, colorChangeTime);
    }
    
    public void setColorToOrange()
    {
        //nMaterial.SetColor("_Color", Color.red);
        nMaterial.DOColor(Color.red, colorChangeTime);
        agroMaterial.DOColor(Color.red, colorChangeTime);
        defaultMaterial.DOColor(Color.red, colorChangeTime);
    }
    
    public void setColorToYellow()
    {
        nMaterial.SetColor("_Color", Color.yellow);
        nMaterial.DOColor(Color.yellow, colorChangeTime);
        agroMaterial.DOColor(Color.yellow, colorChangeTime);
        defaultMaterial.DOColor(Color.yellow, colorChangeTime);
    }
    
    public void setColorToWhite()
    {
        //nMaterial.SetColor("_Color", Color.white);
        nMaterial.DOColor(Color.white, colorChangeTime);
        agroMaterial.DOColor(Color.white, colorChangeTime);
        defaultMaterial.DOColor(Color.white, colorChangeTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
