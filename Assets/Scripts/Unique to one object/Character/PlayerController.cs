using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // MainActions mainActions;

    public CharacterModel characterModel;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        // playerInput.GetComponent<PlayerInput>();
        
        playerInput.actions.FindAction("Jump").performed += aContext => characterModel.Jump();
        playerInput.actions.FindAction("Interact").performed += aContext => characterModel.Interact();
        playerInput.actions.FindAction("Pickup").performed += aContext => characterModel.PickUpCheck();
        // playerInput.actions.FindAction("Cry").performed += aContext => characterModel.CryCoroutine();
        
        playerInput.actions.FindAction("Movement").performed += OnMovementOnperformed;
        playerInput.actions.FindAction("Movement").canceled += OnMovementOnperformed;

            
        // Old code because the PlayerInput class doesn't support autogenerated c# yet
        
        // mainActions = new MainActions();
        // mainActions.InGame.Enable();
        //
        // mainActions.InGame.Jump.performed += aContext => characterModel.Jump();
        // mainActions.InGame.Interact.performed += aContext => characterModel.Interact();
        // mainActions.InGame.Pickup.performed += aContext => characterModel.PickUp();
        //
        // mainActions.InGame.Movement.performed += OnMovementOnperformed;
        // mainActions.InGame.Movement.canceled += OnMovementOnperformed;
    }

    private void OnMovementOnperformed(InputAction.CallbackContext aContext)
    {
        if (aContext.phase == InputActionPhase.Performed)
        {
            characterModel.MoveDirection(aContext.ReadValue<Vector2>());
            // characterModel.movementDirection = aContext.ReadValue<Vector2>();
        }
        else if(aContext.phase == InputActionPhase.Canceled)
        {
            characterModel.MoveDirection(Vector2.zero);
            // characterModel.movementDirection = Vector2.zero;
        }
    }
}