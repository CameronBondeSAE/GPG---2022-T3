using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// base.OnInspectorGUI();

		if (GUILayout.Button("Open") && Application.isPlaying)
		{
			(target as Door)?.OpenClientRpc();
		}
		if (GUILayout.Button("Close") && Application.isPlaying)
		{
			(target as Door)?.CloseClientRpc();
		}
	}
}
