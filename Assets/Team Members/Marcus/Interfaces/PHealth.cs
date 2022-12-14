using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHealth : MonoBehaviour
{
    public delegate void OnDamaged(float incDamage);
    public event OnDamaged damagedEvent;
    
    public void Damaged(float amount)
    {
        damagedEvent?.Invoke(amount);
    }
}
