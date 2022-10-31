using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alex;
using UnityEditor;

[CustomEditor(typeof(FollowPath), true)]
public class FindPathForAIEditor : Editor
{
    private Grid grid;
    private Node node;
    private AStar astar;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Find Path"))
        {
            FollowPath followPath = (target as FollowPath);

            followPath?.StartCoroutine(followPath?.FindPathToTarget(astar.grid.startPos, astar.grid.endPos));
        }
        
    }
}
