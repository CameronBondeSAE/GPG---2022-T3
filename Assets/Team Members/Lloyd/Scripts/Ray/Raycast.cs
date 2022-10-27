using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lloyd
{


    public class Raycast : MonoBehaviour
    {    
        [SerializeField] float _rayDist;

        private Ray _newRay;

        [SerializeField] private int _bounceNo;

        //new origin
        private Vector3 _newOrigin;

        private Vector3 _newDirection;

        private Vector3 _reflectDir;

        private RaycastHit _newHitInfo;

        // Update is called once per frame
        void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo = new RaycastHit();
            

            if (Physics.Raycast(ray, out hitInfo, _rayDist))
            {
                _reflectDir  = Vector3.Reflect(ray.direction, hitInfo.normal);
                _newOrigin = hitInfo.point;

                _newDirection = _reflectDir;

                BounceRay();
            }
        }

        private void BounceRay()
        {
            for (int x = 0; x < _bounceNo; x++)
            {
                _newRay = new Ray(_newOrigin, _newDirection);
                if (Physics.Raycast(_newRay, out _newHitInfo, _rayDist))
                {
                    _reflectDir = Vector3.Reflect(_newRay.direction, _newHitInfo.normal);
                    _newOrigin = _newHitInfo.point;

                    _newDirection = _reflectDir;
                }
            }

        }
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
