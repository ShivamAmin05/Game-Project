using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

public class DashMovement : MonoBehaviour
{
    // public InputAction playerControls;
    // public PlayerInput playerInput;
    // public PlayerInput.PlayerActions player;
    public Transform orientation;
    public Transform camPos;
    public Camera playerCam;
    private Rigidbody rb;
    private PlayerMovement movement;

    [SerializeField] float dashForce;
    [SerializeField] float groundDashMultiplier;
    [SerializeField] float dashUpwardForce;
    // [SerializeField] float dashDuration;
    [SerializeField] int setDashes;
    public int dashNum;

    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float dashFov;

    private Vector3 direction = Vector2.zero;
    Animator playerAnimator;
    GameObject playerBody;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
        playerBody = GameObject.Find("PlayerBody");
        playerAnimator = playerBody.GetComponent<Animator>();
    }
    private void Update()
    {
        
        direction = getDirection();
        // moveDirection = player.Movement.ReadValue<Vector2>();  
        if(movement.isGrounded)
        {
            dashNum = 0;
        }
    }   

    public void Dash()
    { 
        // playerAnimator.ResetTrigger("Dash");
        if(movement.isGrounded == false)
        {
            // diveded dashNum by 3 because every time dash is called unity increments dashNum by 3
            if(dashNum/3 < setDashes)
            {
                cam.fieldOfView = dashFov;
                Vector3 dashingForce = direction * dashForce + orientation.up * dashUpwardForce;
                rb.AddForce(dashingForce, ForceMode.Impulse);
                dashNum++;
            }
        }
        else
        {
            cam.fieldOfView = dashFov;
            Vector3 dashingForce = direction * dashForce * groundDashMultiplier;
            // print(dashingForce);
            rb.AddForce(dashingForce, ForceMode.Impulse);
            print("slide");
            playerAnimator.SetTrigger("Dash");
        }
        
    }
    public Vector3 getDirection()
    {
        
        Vector3 direction = new Vector3();
        // if(movement.moveDirection.x == 0 && movement.moveDirection.y == 0)
        // {
            direction = camPos.forward;
            // print(direction);
        // else
        // {
        //     direction = orientation.forward * movement.moveDirection.z + orientation.right * movement.moveDirection.x;
        //     print(orientation.forward);
        // }
        return direction.normalized;
    }

    // Update is called once per frame
    // void OnEnable()
    // {
    //     playerControls.Enable();
    // }
    // void OnDisable()
    // {
    //     playerControls.Disable()
    // }
}
