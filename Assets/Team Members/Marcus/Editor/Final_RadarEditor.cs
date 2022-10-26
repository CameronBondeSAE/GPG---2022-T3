using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScanningRadar))]
public class Final_RadarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Start Scan") && Application.isPlaying)
        {
            (target as ScanningRadar)?.StartScan();
        }
    }
}