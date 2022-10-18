using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MarcusInput : MonoBehaviour
{
    public Material[] playerColour;
    private MeshRenderer mr;

    private Teamgame controller;
    private Vector2 playerMove;
    private Rigidbody rb;
    public float speed;

    public GameObject flames;

    private void OnEnable()
    {
        controller = new Teamgame();
        controller.Player.Enable();

        controller.Player.Move.performed += OnPlayerMove;
        controller.Player.Move.canceled += OnPlayerMove;

        controller.Player.Throw.performed += OnPlayerThrow;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        
        mr.material = playerColour[Random.Range(0, playerColour.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVelocity = new Vector3(playerMove.x,
                                           0,
                                           playerMove.y);
        rb.AddForce(playerVelocity * speed * (Time.deltaTime / 2f));
    }
    
    private void OnPlayerMove(InputAction.CallbackContext input)
    {
        playerMove = input.ReadValue<Vector2>();
    }

    private void OnPlayerThrow(InputAction.CallbackContext input)
    {
        //I forgot this was a flame thrower
        //I could use a particle effect with a trigger collider
        //and somehow track the rotation of the player with the mouse
        //and toggle the collision check when this event is performed and cancelled
    }
}
