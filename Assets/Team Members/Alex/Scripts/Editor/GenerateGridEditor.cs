using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alex;
using UnityEditor;

[CustomEditor(typeof(GridGenerator), true)]
public class GridGeneratorEditor : Editor
{
    private Grid grid;
    private Node node;
    private AStar astar;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Generate Grid"))
        {
            GridGenerator.singleton.Scan();
        }
        
    }
}
