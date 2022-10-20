using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
        public float HP;
        
        public float MyHP()
        {
                return HP;
        }

        //determines how flammable something is (ie, the higher the float, the faster x will burn)
        public float fireDamage;
        
        public float MyFireDamage()
        {
                return fireDamage;
        }
}
