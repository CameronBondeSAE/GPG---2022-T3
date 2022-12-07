using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ManEaterView : MonoBehaviour
{
    public Animator maneaterAnim;
    
    public ManEater manEater;
    public GameObject bulb;
    public GameObject stalk;

    private Material[] bodyMaterial;

    private void OnEnable()
    {
        manEater.maneaterDeathEvent += Dying;
        manEater.maneaterBurnEvent += Burning;
    }

    // Start is called before the first frame update
    void Start()
    {
        bodyMaterial = new Material[2];
        bodyMaterial[0] = bulb.GetComponent<Renderer>().material;
        bodyMaterial[1] = stalk.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Dying()
    {
        maneaterAnim.SetBool("isDying", true);
        
        //Shrink and shake
        transform.DOShakePosition(2f, 0.25f, 1, 25f);
        transform.DOScale(new Vector3(0.5f, 0, 0.5f), 3f);
    }

    void Burning()
    {
        //Tween to black
        foreach (Material item in bodyMaterial)
        {
            item.DOColor(Color.black, 5f);
        }
    }
}
