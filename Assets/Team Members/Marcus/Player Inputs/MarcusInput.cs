using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MarcusInput : MonoBehaviour, IControllable
{
    public Material[] playerColour;
    private MeshRenderer mr;

    private Teamgame controller;
    private Vector2 playerMove;
    private Rigidbody rb;
    public float speed;

    public ParticleSystem ps;
    private bool firing;

    private void OnEnable()
    {
        controller = new Teamgame();
        controller.Player.Enable();

        controller.Player.Move.performed += OnPlayerMove;
        controller.Player.Move.canceled += OnPlayerMove;

        controller.Player.Throw.performed += OnPlayerThrow;
        controller.Player.Throw.canceled += OnPlayerThrow;
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

    //With the new flamethrower mechanics I could add this here rather than a separate script
    /*public GameObject fireball;
    private Vector3 flameSpeed;

    void CreateFlame()
    {
        gameObject flame = Instantiate(fireball, transform.forward, Quaternion.identity);
        flame.GetComponent<Rigidbody>().AddForce(flameSpeed)
    }*/
    public delegate void Firing();
    public event Firing FireEvent;

    public delegate void Stopped();
    public event Stopped StopEvent;
    
    private void OnPlayerThrow(InputAction.CallbackContext input)
    {
        if (!firing)
        {
            ps.Play();
            FireEvent?.Invoke();
        }
        else
        {
           ps.Stop(); 
           StopEvent?.Invoke();
        }
        
        firing = !firing;
    }

    public void Move(Vector2 direction)
    {

    }

    public void Aim(Vector2 direction)
    {

    }

    public void Action1()
    {

    }

    public void Action2()
    {

    }

    public void Action3()
    {
       
    }

    public void Action4Performed()
    {
        
    }

    public void Action4Cancelled()
    {

    }

    public void Action5Performed()
    {

    }

    public void Action5Cancelled()
    {

    }

    public void Action4()
    {
       
    }

    public void Action5()
    {
        
    }
}
