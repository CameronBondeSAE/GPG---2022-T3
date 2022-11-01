using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alex;
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


            if (aStar != null)
            {
                aStar.grid.Scan();
                aStar.FindPathStartCoroutine(aStar.startPos, aStar.endPos);
            }
        }
        
    }
}
