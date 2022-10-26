using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Radar))]
public class RadarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Start Radar") && Application.isPlaying)
        {
            (target as Radar)?.RadialScan();
        }
    }
}
