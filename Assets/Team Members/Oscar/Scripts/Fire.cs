using Oscar;
using UnityEngine;

public class Fire : MonoBehaviour
{
    //private Explosive_Model _explosiveModelHurt;
    
    private void OnTriggerEnter(Collider other)
    {
        //if the collision is with something that has IFlammable on it
        //set it on fire!
        if (other.GetComponent<IFlammable>() != null)
        {
            other.GetComponent<IFlammable>().SetOnFire();
        }
    }
}
