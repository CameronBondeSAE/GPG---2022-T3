using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


/// <summary>
/// You can't alter a list WHILE you're for eaching through it. So you can make a 2nd list.
/// </summary>
public class ModifyListsWithoutErrors : MonoBehaviour
{
	public List<Transform> resourcesInSight;

	[Button]
	public void RemoveUsingLinqAndLamdba()
    {
	    resourcesInSight.RemoveAll(transformToTest => transformToTest == null);
    }

    [Button]
    public void RemoveUsingTwoLists()
    {
	    List<int> thingsToRemove = new List<int>();
	    for (var index = 0; index < (resourcesInSight).Count; index++)
	    {
		    var resource = (resourcesInSight)[index];
		    if (resource == null)
			    thingsToRemove.Add(index);
	    }

	    foreach (int index in thingsToRemove)
	    {
		    resourcesInSight.RemoveAt(index);
	    }
    }
}
