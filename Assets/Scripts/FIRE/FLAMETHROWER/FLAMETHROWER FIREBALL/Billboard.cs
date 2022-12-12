using System.Collections;
using System.Collections.Generic;
using Luke;
using UnityEngine;
public class Billboard : MonoBehaviour
{
    public Transform _cameraTransform;
    private Transform _myTransform;

    // Use this for initialization
    void Start()
    {
        _myTransform = transform;
        _cameraTransform = GameManager.singleton.cameraBrain.OutputCamera.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
            _myTransform.forward = _cameraTransform.forward;
            
            if(transform.parent != null) _myTransform.LookAt(_cameraTransform, transform.parent.right);
    }
}
