using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
public class GameManager : MonoBehaviour
{
	public static GameManager Singleton;
	
	public event Action OnGameStart;
	public event Action OnGameEnd;

	public void InvokeOnGameStart()
	{
		OnGameStart?.Invoke();
	}

	public void InvokeOnGameEnd()
	{
		OnGameEnd?.Invoke();
	}

	void Awake()
	{
		Singleton = this;
	}
	
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}
