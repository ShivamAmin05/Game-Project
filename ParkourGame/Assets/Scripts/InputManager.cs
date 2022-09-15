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
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;
        // move = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        move = GetComponent<PlayerMovement>();
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
