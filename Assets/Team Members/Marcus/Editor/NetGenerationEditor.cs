using System.Collections;
using System.Collections.Generic;
using Marcus;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetworkedGenerator))]
public class NetGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Perlin") && Application.isPlaying)
        {
            (target as NetworkedGenerator)?.SpawnPerlin();
        }
        
        if (GUILayout.Button("Generate Bases") && Application.isPlaying)
        {
            (target as NetworkedGenerator)?.SpawnBases();
        }
        
        if (GUILayout.Button("Generate Border") && Application.isPlaying)
        {
            (target as NetworkedGenerator)?.SpawnBorderClientRpc();
        }
        
        if (GUILayout.Button("Generate Items") && Application.isPlaying)
        {
            (target as NetworkedGenerator)?.SpawnItems();
        }
        
        if (GUILayout.Button("Generate AI") && Application.isPlaying)
        {
            (target as NetworkedGenerator)?.SpawnAI();
        }
        
        if (GUILayout.Button("Generate Explosives (INACTIVE)") && Application.isPlaying)
        {
            (target as NetworkedGenerator)?.SpawnExplosives();
        }

        if (GUILayout.Button("RANDOMISE PERLINS") && Application.isPlaying)
        {
            (target as NetworkedGenerator)?.RandomiseValues();
        }
    }
}
