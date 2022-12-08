using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGUI : MonoBehaviour
{
	private HostOrJoinGUI _hostOrJoinGui;
	public string playerName;
	public int sceneCount;
	public List<string> scenes;

	public string selectedLevel;

	public List<string> playerNames = new ();
	
	private void OnEnable()
	{
		sceneCount = SceneManager.sceneCountInBuildSettings;
		scenes = new(sceneCount);
		for (int i = 0; i < sceneCount; i++)
		{
			scenes.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
		}
		playerNames.Add(playerName);
	}

	void OnGUI()
	{
		_hostOrJoinGui = gameObject.GetComponent<HostOrJoinGUI>();

		GUILayout.BeginArea(new Rect(Screen.width/2f - Screen.width/2.5f, 20, Screen.width*2/2.5f, 300));
		if (selectedLevel != "")
		{
			if (GUILayout.Button("Start Game"))
			{ 
				Debug.Log("Start Game");
			}
		}
		else
		{
			GUILayout.Label("Select a level");
		}

		GUILayout.Space(20);

		GUILayout.BeginVertical();
		
		GUILayout.BeginHorizontal();
		
		if (playerName == "")
		{
			GUILayout.Label(" Enter your name");
		}
		
		playerName = GUILayout.TextField(playerName, 22);

		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		
		GUILayout.BeginHorizontal();

		GUILayout.Label("Level: ");

		if (selectedLevel == "")
		{
			GUILayout.Label("None");
		}
		else
		{
			GUILayout.Label(selectedLevel);
		}
		
		GUILayout.EndHorizontal();
		
		foreach (string sceneName in scenes)
		{
			if (GUILayout.Button(sceneName)) selectedLevel = sceneName;
		}
		
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();

		foreach (string pName in playerNames)
		{
			GUILayout.Label(pName);
		}
		
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		
		GUILayout.Space(20);

		if (GUILayout.Button("Back"))
		{
			_hostOrJoinGui.enabled = true;
			_hostOrJoinGui.playerName = playerName;
			enabled = false;
		}

		GUILayout.EndVertical();

		GUILayout.EndArea();
	}
}