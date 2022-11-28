using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lloyd
{
    [CustomEditor(typeof(Lloyd.FlamethrowerModel))]
    public class FlamethrowerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Shoot Fire"))
            {
                (target as Lloyd.FlamethrowerModel)?.ShootFire();
            }

            if (GUILayout.Button("Spray Water"))
            {
                (target as Lloyd.FlamethrowerModel)?.SprayWater();
            }
            
            if (GUILayout.Button("Shoot AltFire"))
            {
                (target as Lloyd.FlamethrowerModel)?.ShootAltFire();
            }
            
            if (GUILayout.Button("Shoot Until Dead"))
            {
                (target as Lloyd.FlamethrowerModel)?.ShootUntilDead();
            }
            
            if (GUILayout.Button("Explode"))
            {
                (target as Lloyd.FlamethrowerModel)?.DestroySelf();
            }
        }
    }
    
    [CustomEditor(typeof(Flammable))]
    public class FlammableComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //if (GUILayout.Button("Burn"))
            {
               //(target as Flammable)?.ChangeHeat(IHeatSource x, 25f);
            }
            
            if (GUILayout.Button("Extinguish"))
            {
                (target as Flammable)?.Extinguish();
            }
        }
    }
}