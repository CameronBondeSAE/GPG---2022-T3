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
    public TMP_Text alienScoreText;
    public int goalScore;
    public int alienGoalScore;
    private Checkpoint humanCheckpoint;
    private Checkpoint alienCheckpoint;

    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();
	    
	    gameStarted = true;
	    if (IsServer) AssignCheckpoint();
    }

    void AssignCheckpoint()
    {
	    foreach (var hq in FindObjectsOfType<HQ>())
	    {
		    if (hq.type == HQ.HQType.Humans)
		    {
			    humanCheckpoint = hq.GetComponentInChildren<Checkpoint>();
            }

            if (hq.type == HQ.HQType.Aliens)
            {
                alienCheckpoint = hq.GetComponentInChildren<Checkpoint>();
            }
	    }

        alienCheckpoint.itemPlacedEvent += UpdateAlienDepositedScore;
	    humanCheckpoint.itemPlacedEvent += UpdateHumanDepositedScore;
        goalScore = GameManager.singleton.targetEndResources;
        
        //HACK: replace * 3 with a smarter score
        alienGoalScore = GameManager.singleton.targetEndResources * 3;
    }

    void UpdateAlienDepositedScore(int amount)
    {
        UpdateAlienDepositedScoreClientRpc(amount, alienGoalScore);
    }

    [ClientRpc]
    void UpdateAlienDepositedScoreClientRpc(int amount, int alienGoalScore)
    {
        alienScoreText.text = amount.ToString() + "/" + alienGoalScore.ToString();
    }
    
    void UpdateHumanDepositedScore(int amount)
    {
        UpdateHumanDepositedScoreClientRpc(amount, goalScore);
    }

    [ClientRpc]
    void UpdateHumanDepositedScoreClientRpc(int amount, int goalAmount)
    {
        scoreText.text = amount.ToString() + "/" + goalAmount.ToString();
    }

    void Update()
    {
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
