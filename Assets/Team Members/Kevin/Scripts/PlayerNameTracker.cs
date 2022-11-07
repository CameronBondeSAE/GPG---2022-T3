using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kevin
{
    public class PlayerNameTracker : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public TMP_Text playerName;
        
        void Update()
        {
            if (target != null)
            {
                transform.position = target.position + offset;
                //transform.LookAt(target);
            }
        }
    }
}

