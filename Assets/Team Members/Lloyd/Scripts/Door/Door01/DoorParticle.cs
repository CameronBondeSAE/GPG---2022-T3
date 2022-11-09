using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Lloyd;

public class DoorParticle : MonoBehaviour
{
    public ParticleSystem steam01;
    public ParticleSystem steam02;
    public ParticleSystem steam03;
    public ParticleSystem steam04;
    public List<ParticleSystem> steamList = new List<ParticleSystem>();
    private ParticleSystem tempSyst;

    public DoorEventManager _doorEvent;

    private void OnEnable()
    {
        _doorEvent.DoorIdleEvent += SteamOn;

        steamList.Add(steam01);
        steamList.Add(steam02);
        steamList.Add(steam03);
        steamList.Add(steam04);
    }


    private void SteamOn()
    {
        steam01.Play();

        steam02.Play();

        steam03.Play();

        steam04.Play();

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(.25f);
        SteamOff();
    }

    private void SteamOff()
    {
        steam01.Stop();

        steam02.Stop();

        steam03.Stop();

        steam04.Stop();
    }

    private void OnDisable()
    {
        _doorEvent.DoorIdleEvent -= SteamOn;
    }
}