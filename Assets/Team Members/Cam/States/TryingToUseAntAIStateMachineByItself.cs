using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Tanks;
using UnityEngine;

public class TryingToUseAntAIStateMachineByItself : MonoBehaviour
{
	public GameObject camGuy;
	
    // Start is called before the first frame update
    IEnumerator Start()
    {
	    yield return new WaitForSeconds(3f);
	    camGuy.GetComponent<AntAIAgent>().SetState("AttackState", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
