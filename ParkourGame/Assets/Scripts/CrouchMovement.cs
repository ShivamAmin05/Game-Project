using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchMovement : MonoBehaviour
{
    private PlayerMovement move;
    CapsuleCollider hitBox;
    GameObject playerBody;
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<PlayerMovement>();
        playerBody = GameObject.Find("PlayerBody");
        hitBox = playerBody.GetComponent<CapsuleCollider>();
        playerAnimator = playerBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void startCrouch()
    {
        if(move.isGrounded)
        {
            move.currSpeed = move.crouchSpeed;
            move.desiredMoveSpeed = move.crouchSpeed;
            hitBox.height = move.crouchHeight;
            hitBox.center = new Vector3(0f,0.5f,0f);
            playerAnimator.SetBool("isCrouching", true);
        }
    }
    public void endCrouch()
    {
        move.currSpeed = move.standSpeed;
        hitBox.height = move.standHeight;
        hitBox.center = new Vector3(0f,0.8f,0f);
        playerAnimator.SetBool("isCrouching", false);
    }
}