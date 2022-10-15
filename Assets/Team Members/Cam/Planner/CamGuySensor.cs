using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

public enum CamGuy
{
	hasHealth = 0,
	attackingPlayer = 1
}

public class CamGuySensor : MonoBehaviour, ISense
{

    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
	    aWorldState.Set(CamGuy.hasHealth, GetComponent<Health>().amount > 50f);
	    aWorldState.Set(CamGuy.attackingPlayer, false);
    }
}
