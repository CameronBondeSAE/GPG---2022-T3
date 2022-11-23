using System;
using System.Collections;
using System.Collections.Generic;
using Oscar;
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
                
                if (GUILayout.Button("Perlin Spawn") && Application.isPlaying)
                {
                        (target as MapGenerator)?.SpawnPerlinClientRpc();
                }
                
                if (GUILayout.Button("Border Spawn") && Application.isPlaying)
                {
                        (target as MapGenerator)?.SpawnBorderClientRpc();
                }
                
                if (GUILayout.Button("AI Spawn") && Application.isPlaying)
                {
                        (target as MapGenerator)?.SpawnAIClientRpc();
                }
                
                if (GUILayout.Button("Explosives Spawn") && Application.isPlaying)
                {
                        (target as MapGenerator)?.SpawnExplosivesClientRpc();
                }
                
                if (GUILayout.Button("Items Spawn") && Application.isPlaying)
                {
                        (target as MapGenerator)?.SpawnItemsClientRpc();
                }
                
                if (GUILayout.Button("HQ Spawn") && Application.isPlaying)
                {
                        (target as MapGenerator)?.SpawnBasesClientRpc();
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
                        (target as PerlinCube_Model)?.DestroyTheWallClientRpc();
                }
        }
}

[CustomEditor(typeof(Radar_Model))]
public class AnotherButtonEditor : Editor
{
        public override void OnInspectorGUI()
        {
                base.OnInspectorGUI();

                if (GUILayout.Button("Radar Is Equipted") && Application.isPlaying)
                {
                        (target as Radar_Model)?.pickedUp100();
                }
                if (GUILayout.Button("Radar Is Not On") && Application.isPlaying)
                {
                        (target as Radar_Model)?.NotOn();
                }
        }
}