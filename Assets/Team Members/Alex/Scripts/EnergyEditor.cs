using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Alex
{
    [CustomEditor(typeof(Energy), true)]
    public class EnergyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            Energy energy = (target as Energy);

            if (GUILayout.Button("Give Energy"))
            {
                energy.energyAmount += 10;
            }

            if (GUILayout.Button("Take Energy"))
            {
                energy.energyAmount -= 10;
            }

            if (GUILayout.Button("Full Energy"))
            {
                energy.energyAmount += 10000000;
            }

            if (GUILayout.Button("Suck Energy"))
            {
                energy.energyAmount -= 10000000;
            }

            GUILayout.EndHorizontal();
        }
    }
}