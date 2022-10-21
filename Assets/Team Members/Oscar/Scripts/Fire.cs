using Oscar;
using UnityEngine;

public class Fire : MonoBehaviour
{
    //private Explosive_Model _explosiveModelHurt;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Explosive_Model>())
        {
            // _explosiveModelHurt = other.GetComponent<Explosive_Model>();
            // _explosiveModelHurt.SetOnFire();
        }
    }
}
