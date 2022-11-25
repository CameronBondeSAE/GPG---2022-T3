   using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHP;
    public float HP;

    private bool isAlive=true;

    public HealthView healthView;

    private void OnEnable()
    {
        HP = maxHP;
        healthView = GetComponentInChildren<HealthView>();
        
        healthView.SetColor(Color.white);
        healthView.ChangeHP(maxHP);
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
                HP = 0;
                isAlive = false;
                OnYouDied();
            }

            healthView.ChangeHP(HP);
        }
    }

    public float GetHP()
    {
        return HP;
    }

    [Button]
    public void ChangeHealthButton()
    {
        healthView.ChangeHP(HP);
    }

    [Button]
    public void Die()
    {
        ChangeHP(-1000000000);
    }

    public event Action<float> ChangeHealth; 

    private void OnChangeHealth(float x)
    {
        ChangeHealth?.Invoke(x);
    }
    
    public event Action YouDied;

    private void OnYouDied()
    {
        YouDied?.Invoke();
    }

}