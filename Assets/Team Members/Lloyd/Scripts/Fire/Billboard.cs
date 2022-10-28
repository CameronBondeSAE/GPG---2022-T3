using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Billboard : MonoBehaviour
{
    public Transform _cameraTransform;
    private Transform _myTransform;

    // Use this for initialization
    void Start()
    {
        _myTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
            _myTransform.forward = _cameraTransform.forward;
            
            _myTransform.LookAt(_cameraTransform, Vector3.up);
    }
}
