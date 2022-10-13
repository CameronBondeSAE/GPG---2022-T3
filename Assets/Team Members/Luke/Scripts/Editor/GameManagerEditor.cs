using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Luke
{
[CustomEditor(typeof(Luke.GameManager))]
public class GameManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Game Start"))
		{
			((GameManager) target)?.InvokeOnGameStart();
		}
		
		if (GUILayout.Button("Game End"))
		{
			((GameManager) target)?.InvokeOnGameEnd();
		}
	}
}
}