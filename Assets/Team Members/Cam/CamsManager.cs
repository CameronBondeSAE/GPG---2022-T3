using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamsManager : MonoBehaviour
{
	public static CamsManager singleton; // Also commonly 'instance'
	
	public int things;
	public string stuff;

	public void Awake()
	{
		singleton = this;
	}

	public void DoThings()
	{
		
	}

	public void DoStuff()
	{
		
	}
}
