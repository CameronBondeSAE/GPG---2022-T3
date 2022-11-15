using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapButtonEditor : Editor
{
        public override void OnInspectorGUI()
        {
                base.OnInspectorGUI();

                if (GUILayout.Button("Terrain Refresh") && Application.isPlaying)
                {
                        (target as MapGenerator)?.ResetTheMap();
                }

        }
}

[CustomEditor((typeof(PerlinCube_Model)))]
public class TheButtonEditor : Editor
{
        public override void OnInspectorGUI()
        {
                base.OnInspectorGUI();

                if (GUILayout.Button("Start Wall Destruction") && Application.isPlaying)
                {
                        (target as PerlinCube_Model)?.DestroyTheWall();
                }
        }
}