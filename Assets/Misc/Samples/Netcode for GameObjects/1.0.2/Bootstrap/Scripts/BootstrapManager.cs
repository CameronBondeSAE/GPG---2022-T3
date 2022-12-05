using System;
using UnityEngine;

namespace Unity.Netcode.Samples
{
    /// <summary>
    /// Class to display helper buttons and status labels on the GUI, as well as buttons to start host/client/server.
    /// Once a connection has been established to the server, the local player can be teleported to random positions via a GUI button.
    /// </summary>
    public class BootstrapManager : MonoBehaviour
    {
	    public bool autoHost = false;
	    private void Awake()
	    {
		    Time.timeScale = 0; // HACK to stop everything so you can host/join
		    Debug.Log("STUFF");
		    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
	    }

	    private void OnClientConnectedCallback(ulong obj)
	    {
		    if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsList.Count > 1)
		    {
			    Time.timeScale = 1; // HACK
		    }
	    }


	    private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            var networkManager = NetworkManager.Singleton;
            if (!networkManager.IsClient && !networkManager.IsServer)
            {
                if (GUILayout.Button("Host") || autoHost == true)
                {
                    networkManager.StartHost();
                }

                if (GUILayout.Button("Client"))
                {
                    networkManager.StartClient();
                    Time.timeScale = 1; // HACK
                }

                if (GUILayout.Button("Server"))
                {
                    networkManager.StartServer();
                }
            }
            else
            {
                GUILayout.Label($"Mode: {(networkManager.IsHost ? "Host" : networkManager.IsServer ? "Server" : "Client")}");


                // // "Random Teleport" button will only be shown to clients
                // if (networkManager.IsClient)
                // {
                //     if (GUILayout.Button("Random Teleport"))
                //     {
                //         if (networkManager.LocalClient != null)
                //         {
                //             // Get `BootstrapPlayer` component from the player's `PlayerObject`
                //             if (networkManager.LocalClient.PlayerObject.TryGetComponent(out BootstrapPlayer bootstrapPlayer))
                //             {
                //                 // Invoke a `ServerRpc` from client-side to teleport player to a random position on the server-side
                //                 bootstrapPlayer.RandomTeleportServerRpc();
                //             }
                //         }
                //     }
                // }
            }

            GUILayout.EndArea();
        }
    }
}
