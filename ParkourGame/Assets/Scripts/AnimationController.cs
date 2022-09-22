using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerMovement move;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(move.moveDirection.x == 0 && move.moveDirection.y == 0)
        {
            playerAnimator.SetBool("isMoving", false);
        }
        else
        {
            playerAnimator.SetBool("isMoving", true);
        }
    }
}
