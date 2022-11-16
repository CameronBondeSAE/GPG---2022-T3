using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHP;
    public float HP;

    private bool isAlive=true;

    private void OnEnable()
    {
        HP = maxHP;
    }

    public void ChangeHP(float amount)
    {
        if (isAlive)
        {
            HP += amount;

            if (HP >= maxHP)
                HP = maxHP;

            if (HP <= 0)
            {
                isAlive = false;
                YouDiedFunction();
            }
        }
    }

    public float GetHP()
    {
        return HP;
    }
    
    public event Action YouDiedEvent;

    private void YouDiedFunction()
    {
        YouDiedEvent?.Invoke();
    }

}
