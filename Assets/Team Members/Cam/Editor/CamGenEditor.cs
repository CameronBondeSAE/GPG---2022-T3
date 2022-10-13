using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CamGen))]
public class CamGenEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Generate"))
		{
			(target as CamGen).Generate();
		}

		// Vector3 randomStartingPoint = new Vector3();
		// randomStartingPoint.x/z = // random

			// for loop stuff
		// Vector3 perlin = new Vector3();
		// perlin.x = Mathf.PerlinNoise(x + randomStartingPoint.x,0,z)
			
		
	}
}
