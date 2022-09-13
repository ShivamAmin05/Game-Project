using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.LandActions land;
    private PlayerMovement move;
    private PlayerLook look;
    private WallRun wallRun;

    void Awake()
    {
        playerInput = new PlayerInput();
        land = playerInput.Land;
        move = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        wallRun = GetComponent<WallRun>();
        // land.Crouch.performed += ctx => move.Crouch();
        land.Jump.performed += ctx => move.Jump();
        land.Jump.performed += ctx => wallRun.WallJump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move.MovePlayer(land.Movement.ReadValue<Vector2>());
        
        if(land.Sprint.ReadValue<float>() > 0)
        {
            move.Sprint(true);
        }
        if(land.Sprint.ReadValue<float>() == 0)
        {
            move.Sprint(false);
        }
        // if(land.Jump.ReadValue<float>() > 0 && land.Jump.triggered)
        //     wallRun.WallJump();
        
    }
    
    private void LateUpdate() 
    {
        look.ProcessLook(land.Look.ReadValue<Vector2>());
    }
    private void OnEnable() 
    {
        land.Enable();
    }
    private void OnDisable() 
    {
        land.Disable();
    }
}
