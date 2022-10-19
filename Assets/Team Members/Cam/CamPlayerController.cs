using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamPlayerController : MonoBehaviour
{
	public PlayerInput playerInput;
	public Vector2 value;
	public Vector2 moveDirection;

	// Start is called before the first frame update
    void Start()
    {
	    GuyDudeActionMap guyDudeActionMap = new GuyDudeActionMap();
	    guyDudeActionMap.Player.Enable();
	    
	    guyDudeActionMap.Player.Move.performed += MoveOnperformed;
	    guyDudeActionMap.Player.Move.canceled += MoveOnperformed;
    }

    private void MoveOnperformed(InputAction.CallbackContext obj)
    {
		moveDirection = obj.ReadValue<Vector2>();
    }

    private void FireOnperformed(InputAction.CallbackContext obj)
    {
	    Debug.Log(obj.started);
	    Debug.Log(obj.canceled);
	    Debug.Log(obj.phase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
