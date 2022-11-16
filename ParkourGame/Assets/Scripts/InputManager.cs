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
    private PlayerMovement crouch;
    private PlayerLook look;
    private WallRunning wallRun;
    private DashMovement dash;
    GameObject camHolder;
    GameObject playerBody;
    CapsuleCollider hitBox;
    Animator playerAnimator;

    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;
        look = GetComponent<PlayerLook>();
        move = GetComponent<PlayerMovement>();
        crouch = GetComponent<PlayerMovement>();
        wallRun = GetComponent<WallRunning>();
        playerBody = GameObject.Find("PlayerBody");
        hitBox = playerBody.GetComponent<CapsuleCollider>();
        playerAnimator = playerBody.GetComponent<Animator>();
;       player.Jump.performed += ctx => wallRun.WallJump();
        player.Crouch.performed += Crouch;
        player.Crouch.canceled += Crouch;
    }

    void Crouch(InputAction.CallbackContext context)
    {
        if(context.performed) // the key has been pressed
        {
            move.crouchMultiplier = 0.2f;
            hitBox.height = 1f;
            hitBox.center = new Vector3(0f,0.5f,0f);
            playerAnimator.SetBool("isCrouching", true);
        }
        if(context.canceled) //the key has been released
        {
            move.crouchMultiplier = 1f;
            hitBox.height = 1.9f;
            hitBox.center = new Vector3(0f,0.8f,0f);
            playerAnimator.SetBool("isCrouching", false);
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
    }
    private void OnDisable() 
    {
        player.Disable();
    }
}
