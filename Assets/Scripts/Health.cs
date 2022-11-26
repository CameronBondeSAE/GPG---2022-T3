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

    private void OnEnable()
    {
        OnSpawn();
        ChangeHP(maxHP);
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

            OnChangeHealth(HP);
        }
    }

    public float GetHP()
    {
        return HP;
    }

    [Button]
    public void ChangeHealthButton()
    {
        ChangeHP(0);
    }

    [Button]
    public void Die()
    {
        ChangeHP(-1000000000);
    }

    public void Respawn()
    {
        OnSpawn();
        isAlive = true;
        ChangeHP(maxHP);
    }

    public event Action Spawn; 

    private void OnSpawn()
    {
        Spawn?.Invoke();
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