using System.Collections;
using System.Collections.Generic;
using Luke;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameTimer : NetworkBehaviour
{
    public bool gameStarted;
    public float time;
    public TMP_Text timeText;

    void OnEnable()
    {
        gameStarted = true;
    }
    

    void Update()
    {
        /*if (IsServer)
        {*/
        if (IsServer)
        {
            if (time > 0 && gameStarted)
            {
                if (time < 11)
                {
                    RequestTimerColorChangeServerRPC();
                    //timeText.color = new Color(255, 0, 0,255);
                }

                CountdownTimerServerRPC();
            }
            else
            {
                time = 0; 
                GameManager.singleton.InvokeOnGameEnd();
            }
        }
            /*if (time > 0  && gameStarted)
            {
                if (time < 11)
                {
                    RequestTimerColorChangeServerRPC();
                    //timeText.color = new Color(255, 0, 0,255);
                }
                time -= Time.deltaTime;
                TimeToDisplayServerRPC(time);
            }
            else
            {
                time = 0; 
                GameManager.singleton.InvokeOnGameEnd();
            }*/
            
        
        //}
    }

    [ServerRpc]
    void CountdownTimerServerRPC()
    {
        if (IsServer)
        {
            time -= Time.deltaTime;
            TimeToDisplayServerRPC(time);
        }
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    void TimeToDisplayServerRPC(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.Floor(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        RequestTimeToDisplayClientRPC(time);
    }
    
    
    [ClientRpc]
    void RequestTimeToDisplayClientRPC(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.Floor(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [ServerRpc(RequireOwnership = false)]
    void RequestTimerColorChangeServerRPC()
    {
        timeText.color = new Color(255, 0, 0,255);
        RequestTimerColorChangeClientRPC();
    }
    
    [ClientRpc]
    void RequestTimerColorChangeClientRPC()
    {
        timeText.color = new Color(255, 0, 0,255);
    }
}
