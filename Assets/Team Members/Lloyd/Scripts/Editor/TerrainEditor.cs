using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Lloyd.LevelGenerator))]

public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Randomise Values"))
        {
            (target as Lloyd.LevelGenerator)?.RandomiseValues();
        }

        if (GUILayout.Button("Generate Terrain (Play Mode Only)") && Application.isPlaying)
        {
            (target as Lloyd.LevelGenerator)?.GenerateTerrain();
        }
        
        if (GUILayout.Button("Spawn Walls & Floor (Play Mode Only)") && Application.isPlaying)
        {
            (target as Lloyd.LevelGenerator)?.SpawnBorderClientRpc();
        }
        
        if (GUILayout.Button("Spawn Plants (Play Mode Only)") && Application.isPlaying)
        {
            (target as Lloyd.LevelGenerator)?.SpawnItemsClientRpc();
        }
        
        if (GUILayout.Button("Spawn Bases (Play Mode Only)") && Application.isPlaying)
        {
            (target as Lloyd.LevelGenerator)?.SpawnBasesClientRpc();
        }
    }
}