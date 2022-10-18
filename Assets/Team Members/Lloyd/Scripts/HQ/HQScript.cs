using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Conditions;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;

public class HQScript : MonoBehaviour
{
    private bool isActive;
    
    public enum HQType
    {
        Neutral,
        Humans,
        Aliens
    };
    public HQType myHQType;

    private GameObject myself;
    
    private Renderer rend;

    //tracks items deposited
    private int itemCount;
    //when this many items have been deposited, fire victory event
    public int itemVictory;

    //tracks Team
    //Human or Alien
    //Neutral could be destroyed bases?
    private int HQInt;
    private string HQString;

    private void Start(){
        
        myself = this.GameObject();
        
        HQInt = (int)myHQType;
        HQString = myHQType.ToString();
        itemCount = 0;

        rend = this.GetComponent<Renderer>();

        if (HQInt == 1)
            rend.material.color = Color.green;
            
        else if (HQInt == 2)
            rend.material.color = Color.magenta;

        isActive = true;
    }

    private void OnCollisionEnter(Collision c)
    {
        CubeScript cubeScript = c.gameObject.GetComponent<CubeScript>();

        if ((cubeScript != null) && ((c.gameObject.name != "Ground")) && ((c.gameObject.name != "Wall")))
            cubeScript.KillSelf();
    }

    public void ItemDeposited()
    {
        if (isActive)
        {
            //should only go up if the person depositing is aligned (humans can't deposit at alien base & vice versa)
            itemCount++;

            Debug.Log("Item deposited! " + HQString + " has " + itemCount + " items!");

            if (itemCount >= itemVictory)
                GameOver();
        }
    }

    private void GameOver()
    {
        isActive = false;
        switch (HQInt)
        {
            case 1:
                //humans win
            break;
            
            case 2:
                //aliebs win
                break;
            
        }
        
        Debug.Log("A stunning victory for the "+HQString+"!");
        //Victory Event
    }

    public void SetTeam(int key)
    {
        switch (key)
        {
            case 1:
                myHQType = HQType.Humans;
                break;
            
            case 2:
                myHQType = HQType.Aliens;
                break;
        }
    }
    
    
    void OnEnable()
    {
        EventManager.TerrainClearEvent += KillSelf;
    }

    void OnDisable()
    {
        EventManager.TerrainClearEvent -= KillSelf;
    }

    public void KillSelf()
    {
        Destroy(myself);
    }

    
}