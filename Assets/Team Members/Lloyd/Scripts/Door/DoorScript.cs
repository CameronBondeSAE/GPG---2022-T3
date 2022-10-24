using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Conditions;
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

    private bool isOpen=false;

    private bool isActive=true;

    void OnEnable()
    {
        Lloyd.EventManager.ChangeHealthEvent += ChangeHP;
        Lloyd.EventManager.DoorOpenEvent += FlipActive;
        Lloyd.EventManager.DoorCloseEvent += FlipActive;

        doorPos = this.transform.position;

        HPComp = this.GetComponent<HealthComponent>();

        fireDamage = HPComp.MyFireDamage();
        
        EventManager.ChangeHealthFunction(HPComp.MyHP());
    }
    
    

    public void Interact()
    {
        if (isActive)
        {
            isOpen = !isOpen;
            if (isOpen)
                Lloyd.EventManager.DoorOpenEventFunction();

            else if (!isOpen)
                Lloyd.EventManager.DoorCloseEventFunction();

            isActive = false;
        }
    }

    private void FlipActive()
    {
        
    }

    private void FixedUpdate()
    {
        if (isBurning)
        {
            EventManager.ChangeHealthFunction(-fireDamage);
        }
    }


    public void SetOnFire()
    {
        Lloyd.EventManager.BurningEventFunction();
        isBurning = true;
    }

    public void Burnt()
    {
        Lloyd.EventManager.BurntEventFunction();
        HP = 0;
    }

   

    private void OnDisable()
    {
        Lloyd.EventManager.ChangeHealthEvent -= ChangeHP;
        Lloyd.EventManager.DoorOpenEvent -= FlipActive;
        Lloyd.EventManager.DoorCloseEvent -= FlipActive;
    }

    private void ChangeHP(float amount)
    {
        HP += amount;
        
        if (HP <= 0)
            Burnt();
    }
    
}
}