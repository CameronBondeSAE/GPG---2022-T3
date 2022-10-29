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
}