using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class Health : MonoBehaviour
    {
        public delegate void Damaged(float damageAmount);
        public event Damaged DamageTaken;
        
        void Damage(float incDamage)
        {
            DamageTaken?.Invoke(incDamage);
        }
    }
}
