using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour, IAffectedByVisibilty
{
	public MeshRenderer rend;
	
	private float _timer;

	private void OnEnable()
	{
		rend.enabled = false;
	}

	public void Detection(float timeOnScreen)
	{
		_timer = timeOnScreen;
		rend.enabled = true;
	}

	private void Update()
	{
		_timer -= Time.deltaTime;
		if (_timer <= 0) rend.enabled = false;
	}
}
