using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerInput.PlayerActions player;
    private PlayerMovement move;

    private CrouchMovement crouch;
    private PlayerLook look;
    private WallRunning wallRun;
    private DashMovement dash;


    CapsuleCollider hitBox;
    GameObject playerBody;
    private Animator playerAnimator;

    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;
        look = GetComponent<PlayerLook>();
        move = GetComponent<PlayerMovement>();
        crouch = GetComponent<CrouchMovement>();
        wallRun = GetComponent<WallRunning>();
        dash = GetComponent<DashMovement>();

        playerBody = GameObject.Find("PlayerBody");
        hitBox = playerBody.GetComponent<CapsuleCollider>();
        playerAnimator = playerBody.GetComponent<Animator>();
        
;       player.Jump.performed += ctx => wallRun.WallJump();

        // player.Crouch.performed += Crouch;
        // player.Crouch.canceled += Crouch;

    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if(context.performed) // the key has been pressed
        {
            crouch.startCrouch();
        }
        if(context.canceled) //the key has been released
        {
            crouch.endCrouch();
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if(context.performed) // the key has been pressed
        {
            dash.Dash();
        }
        if(context.canceled) //the key has been released
        {
            dash.ResetDash();
        }
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        move.MovePlayer(player.Movement.ReadValue<Vector2>());
    }
    
    private void LateUpdate() 
    {
        look.camMovement(player.Look.ReadValue<Vector2>());
    }
    private void OnEnable() 
    {
        player.Enable();
        wallRun.enabled = false;
        wallRun.enabled = true;
    }
    private void OnDisable() 
    {
        player.Disable();
    }
}
