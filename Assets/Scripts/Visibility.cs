using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour, IAffectedByVisibility
{
	public GameObject view;

	private float _timer;

	private void OnEnable()
	{
		view.SetActive(false);
	}

	public void Detection(float timeOnScreen)
	{
		_timer = timeOnScreen;
		view.SetActive(true);
	}

	private void Update()
	{
		_timer -= Time.deltaTime;
		if (_timer <= 0) view.SetActive(false);
	}
}