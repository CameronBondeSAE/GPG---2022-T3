using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Luke;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameWaveTimer : NetworkBehaviour
{
    public bool gameStarted;
    public float time;
    public TMP_Text timeText;
    public TMP_Text scoreText;
    private Checkpoint checkpoint;

    void OnEnable()
    {
        gameStarted = true;
        GameManager.singleton.GameHasStartedEvent += AssignCheckpoint;
    }

    void AssignCheckpoint()
    {
        if (IsServer)
        {
            foreach (var hq in FindObjectsOfType<HQ>())
            {
                if (hq.type == HQ.HQType.Humans)
                {
                    checkpoint = hq.GetComponentInChildren<Checkpoint>();
                }
            }
            checkpoint.itemPlacedEvent += UpdateScoreboard;
        }
    }

    void UpdateScoreboard(int amount)
    {
        scoreText.text = amount.ToString();
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
                    int red = 255;
                    RequestTimerColorChangeServerRPC(red);
                    //timeText.color = new Color(255, 0, 0,255);
                }

                CountdownTimerServerRPC();
            }
            else
            {
                int white = 1;
                RequestTimerColorChangeServerRPC(white);
                time = 30; 
                //GameManager.singleton.InvokeOnGameEnd();
                GameManager.singleton.InvokeOnGameWaveTimer();
                //invoke new wave spawner
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
    void RequestTimerColorChangeServerRPC(int color)
    {
        if (color < 2)
        {
            timeText.color = new Color(color, color, color,255);
            RequestTimerColorChangeClientRPC(color);
        }
        else
        {
            timeText.color = new Color(color, 0, 0,color);
            RequestTimerColorChangeClientRPC(color);
        }
    }
    
    [ClientRpc]
    void RequestTimerColorChangeClientRPC(int color)
    {
        if (color < 2)
        {
            timeText.color = new Color(color, color, color,255);
        }
        else
        {
            timeText.color = new Color(color, 0, 0,color);
        }
        
    }
}
