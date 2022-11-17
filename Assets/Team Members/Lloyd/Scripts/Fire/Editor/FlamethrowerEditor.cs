using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lloyd
{
    [CustomEditor(typeof(Lloyd.Flamethrower))]
    public class FlamethrowerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Shoot Fire"))
            {
                (target as Lloyd.Flamethrower)?.ShootFire();
            }
        }
    }
    
    [CustomEditor(typeof(Flammable))]
    public class FlammableComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Burn"))
            {
                (target as Flammable)?.ChangeHeat(25f);
            }
            
            if (GUILayout.Button("Extinguish"))
            {
                (target as Flammable)?.Extinguish();
            }
        }
    }
}