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
                (target as Lloyd.FlamethrowerModel)?.Kill();
            }
        }
    }/*
    
    [CustomEditor(typeof(Flammable))]
    public class FlammableComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Burn"))
            {
                (target as Flammable)?.ChangeHeat(x, 25f);
            }
            
            if (GUILayout.Button("Extinguish"))
            {
                (target as Flammable)?.Extinguish();
            }
        }
    }*/
    
    [CustomEditor(typeof(HQ))]
    public class HQEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Destroy Land"))
            {
                (target as HQ)?.DestroyLand(10);
            }
        }
    }
}