using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorScript : MonoBehaviour, IFlammable
{
    public TMP_Text doorText;
 
    
    
    private HealthComponent HPComp;
    private float HP;

    private float fireDamage;

    private bool isActive;
    
    private bool isBurning;

    private Renderer rend;
    
    

    private void Start()
    {
        HPComp = this.GetComponent<HealthComponent>();

        HP = HPComp.MyHP();

        fireDamage = HPComp.MyFireDamage();
            
        rend = this.GetComponent<Renderer>();
        
        rend.material.SetColor("_BaseColor", new Color(0.3f, 0.4f, 0.6f, 0.3f));

        isActive = true;
        
       // doorText = this.GetComponent<TMP_Text>();

    }

    private void FixedUpdate()
    {
        if (isBurning && isActive)
        {
            rend.material.SetColor("_BaseColor", Color.red);
            EventManager.ChangeHealthFunction(-fireDamage);
        }
        
 
        doorText.text = HP.ToString(); 

        
    }


    public void SetOnFire()
    {
        isBurning = true;
    }

    public void Burnt()
    {
        rend.material.SetColor("_BaseColor", Color.black);
        isActive = false;
        HP = 0;
    }

    private void OnEnable()
    {
        EventManager.ChangeHealthEvent += ChangeHP;
    }

    private void OnDisable()
    {
        EventManager.ChangeHealthEvent -= ChangeHP;
    }

    private void ChangeHP(float amount)
    {
        
        HP += amount;
        
        if (HP <= 0)
            Burnt();
    }
    
    
}
