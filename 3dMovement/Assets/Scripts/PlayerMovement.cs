using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    [SerializeField]Transform orientation; 
    float playerHeight;
    [Header("Movement")]
    public bool isGrounded;
    public float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float acceleration;

    [SerializeField]float moveMultiplier;
    [SerializeField]float airMoveMultiplier;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float jumpForce;
    Vector3 moveDirection;
    [Header("Ground Detection")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField]float groundDistance;
    [Header("Slopes")]
    Vector3 slopeMoveDirection;
    RaycastHit slopeHit;
    public bool onSlope = false;
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {   
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void Update() {
        ControlDrag();
        // ControlSpeed();
        print(rb.velocity);        
        OnSlope();
        // checks if the player is grounded my shooting a ray at thr ground from the bottom of the player
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
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
    public void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x,0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed) 
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x,rb.velocity.y,limitedVel.y);
        }  
    } 
    public void Sprint(bool held)
    {
        if(held == true && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed,sprintSpeed,acceleration * Time.deltaTime);
        }
        if(held == false)
        {
            moveSpeed = Mathf.Lerp(moveSpeed,walkSpeed,acceleration * Time.deltaTime);
        }
    }
    public void MovePlayer(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        if (isGrounded && !OnSlope()){
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier * airMoveMultiplier, ForceMode.Acceleration);
        }
    }
    public void Jump()
    {   
        if(isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void nothing()
    {
        
    }
}
