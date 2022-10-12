using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSpawning))]
public class M_ItemSpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Spawn items") && Application.isPlaying)
        {
            (target as ItemSpawning)?.SpawnItems();
        }
    }
}