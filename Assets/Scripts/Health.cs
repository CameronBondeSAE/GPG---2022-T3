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

    [Button]
    public void Die()
    {
	    ChangeHP(-1000000000);
    }
    
    public event Action YouDiedEvent;

    private void YouDiedFunction()
    {
        YouDiedEvent?.Invoke();
    }

}
