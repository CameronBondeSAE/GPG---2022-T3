using UnityEngine;

public class HostOrJoinGUI : MonoBehaviour
{
	private HostGUI _hostGui;
	// [SerializeField] private JoinGUI joinGui;

	public string playerName;
	
	void OnGUI()
	{
		_hostGui = gameObject.GetComponent<HostGUI>();
		
		GUILayout.BeginArea(new Rect(Screen.width/2f - Screen.width/4f, 20, Screen.width/2f, 300));
		
		if (GUILayout.Button("Host"))
		{
			_hostGui.enabled = true;
			_hostGui.playerName = playerName;
			enabled = false;
		}

		if (GUILayout.Button("Join"))
		{
			Debug.Log("Load Client Scene");
		}

		GUILayout.BeginHorizontal();
		if (playerName == "")
		{
			GUILayout.Label("Type your name -->");
		}

		playerName = GUILayout.TextField(playerName, 22);
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
	}
}