using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationController : MonoBehaviour
{
    private Animator playerAnimator;
    public PlayerMovement movement;
    GameObject player;
    // Start is called before the first frame update
     void Awake() 
    {
        player = GameObject.Find("Player");
        movement = player.GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.moveDirection.x == 0 && movement.moveDirection.z == 0)
        {
            playerAnimator.SetBool("isMoving", false);
        }
        else
        {   
            playerAnimator.SetBool("isMoving", true);
        }
    }
}
