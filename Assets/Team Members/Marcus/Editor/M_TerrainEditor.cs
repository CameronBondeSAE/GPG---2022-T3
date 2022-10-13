using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarcusTerrain))]
public class M_TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Regenerate Terrain") && Application.isPlaying)
        {
            (target as MarcusTerrain)?.ClearMaze();
        }

        if (GUILayout.Button("Spawn items") && Application.isPlaying)
        {
            (target as MarcusTerrain)?.ClearItems();
        }
    }
}
