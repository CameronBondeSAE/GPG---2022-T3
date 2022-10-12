using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnNoise))]
public class ButtonEditor : Editor
{
        public override void OnInspectorGUI()
        {
                base.OnInspectorGUI();

                if (GUILayout.Button("Reset Map") && Application.isPlaying)
                {
                        (target as SpawnNoise)?.ResetTheMap();
                }
                if (GUILayout.Button("Items") && Application.isPlaying)
                {
                        (target as SpawnNoise)?.ItemLocation();
                }
        }
}
