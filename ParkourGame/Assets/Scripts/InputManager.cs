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

    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;
        look = GetComponent<PlayerLook>();
        move = GetComponent<PlayerMovement>();
        crouch = GetComponent<PlayerMovement>();
        wallRun = GetComponent<WallRunning>();
;       player.Jump.performed += ctx => wallRun.WallJump();
        player.Crouch.performed += Crouch;
        player.Crouch.canceled += Crouch;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move.MovePlayer(player.Movement.ReadValue<Vector2>());
        // Crouch(crouch.Crouch());
    }
    void Crouch(InputAction.CallbackContext context)
    {
        if(context.performed) // the key has been pressed
            {
                move.crouchMultiplier = 0.2f;
            }
        if(context.canceled) //the key has been released
        {
            move.crouchMultiplier = 1f;
        }
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
