using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lloyd
{

    public class DoorScript : MonoBehaviour, IFlammable, IInteractable
{

    private HealthComponent HPComp;
    private float HP;

    private float fireDamage;
    
    private bool isBurning;

    Vector3 doorPos;

    private bool isOpen;
    private bool isActive;

    void OnEnable()
    {
        EventManager.ChangeHealthEvent += ChangeHP;

        doorPos = this.transform.position;

        HPComp = this.GetComponent<HealthComponent>();

        fireDamage = HPComp.MyFireDamage();

        isActive = true;
        
        EventManager.ChangeHealthFunction(HPComp.MyHP());
    }
    
    

    public void Interact()
    {
        isOpen = !isOpen;
    }

    private void FixedUpdate()
    {
        if (isBurning && isActive)
        {
            EventManager.ChangeHealthFunction(-fireDamage);
        }
        Debug.Log(HP);
    }


    public void SetOnFire()
    {
        Lloyd.EventManager.BurningEventFunction();
        isBurning = true;
    }

    public void Burnt()
    {
        Lloyd.EventManager.BurntEventFunction();
        isActive = false;
        HP = 0;
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
}