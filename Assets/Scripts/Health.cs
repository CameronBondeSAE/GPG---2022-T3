   using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;
using Sirenix.OdinInspector;
   using Unity.Netcode;

   public class Health : NetworkBehaviour
{
    [SerializeField] private float maxHP;
    public NetworkVariable<float> HP;

    private bool isAlive=true;

    private void OnEnable()
    {
        OnSpawn();
        HP.OnValueChanged += OnValueChanged;
        ChangeHP(maxHP);
    }

    private void OnValueChanged(float previousvalue, float newvalue)
    {
        ChangeHP(newvalue-previousvalue);
    }

    public void ChangeHP(float amount)
    {
        if (isAlive)
        {
            HP.Value += amount;

            if (HP.Value >= maxHP)
                HP.Value = maxHP;

            if (HP.Value <= 0)
            {
                HP.Value = 0;
                isAlive = false;
                OnYouDied();
            }

            OnChangeHealth(HP.Value);
        }
    }

    public float GetHP()
    {
        return HP.Value;
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