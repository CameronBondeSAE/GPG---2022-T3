using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AStar), true)]
public class GeneratePathEditor : Editor
{
    private Grid grid;
    private Node node;
    
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Find Path"))
        {
            AStar aStar = (target as AStar);
            
            aStar?.openNodes.Clear();
            aStar?.openNodes.Clear();
            aStar?.closedNodes.Clear();
            aStar?.isPathable.Clear();
            aStar?.neighbours.Clear();
            aStar?.grid.Scan();
            aStar?.StartCoroutine(aStar?.FindPath(aStar.grid.startPos, aStar.grid.endPos));
        }
        
    }
}
