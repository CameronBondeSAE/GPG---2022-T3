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

                if (GUILayout.Button("Terrain Refresh") && Application.isPlaying)
                {
                        (target as SpawnNoise)?.ResetTheMap();
                }

        }
}

[CustomEditor(typeof(ItemAreaSpawner))]
public class RaycastButton : Editor
{
        public override void OnInspectorGUI()
        {
                base.OnInspectorGUI();

                if (GUILayout.Button("Raycast") && Application.isPlaying)
                {
                        (target as ItemAreaSpawner)?.SpreadItems();
                }
                
        }
}