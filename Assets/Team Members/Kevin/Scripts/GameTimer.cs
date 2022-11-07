using System.Collections;
using System.Collections.Generic;
using Luke;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public bool gameStarted;
    public float time;
    public TMP_Text timeText;
    void Update()
    {
        if (time > 0  && gameStarted)
        {
            if (time < 11)
            {
                timeText.color = new Color(255, 0, 0,255);
            }
            time -= Time.deltaTime;
        }
        else
        {
            time = 0; 
            GameManager.singleton.InvokeOnGameEnd();
        }
        DisplayTime(time);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.Floor(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
