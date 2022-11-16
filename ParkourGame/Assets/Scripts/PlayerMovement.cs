using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [Header("References")]
    [SerializeField]Transform orientation; 
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    private Animator playerAnimator;
    private GameObject playerBody;
    
    [Header("Movement")]
    public float moveSpeed;
    public float crouchMultiplier;
    [SerializeField] float moveMultiplier;
    [SerializeField] float airMoveMultiplier;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float groundDistance;
    public Vector3 moveDirection;
    public bool isGrounded;
    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveDirection = Vector3.zero;
        playerBody = GameObject.Find("PlayerBody");
        playerAnimator = playerBody.GetComponent<Animator>();
        
    }
    private void ControlDrag()
    {   
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
    // }
    private void Update() {
        ControlDrag();
        // speedControl();
        // crouchMultiplier = 1;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded)
        {
            playerAnimator.SetBool("isGrounded",true);
        }
        else 
        {
            playerAnimator.SetBool("isGrounded",false);
        }

    }
 
    public void MovePlayer(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;
        if(moveDirection.x != 0 && moveDirection.z != 0)
        {
            playerAnimator.SetBool("isMoving", true);
            if(isGrounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * crouchMultiplier * moveMultiplier, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * crouchMultiplier * moveMultiplier * airMoveMultiplier, ForceMode.Force);
            }
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }
    }
    public void Jump()
    {
        if(isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetBool("isJumping", true);
        }
        else
        {
            playerAnimator.SetBool("isJumping", false);    
        }
    }
}
