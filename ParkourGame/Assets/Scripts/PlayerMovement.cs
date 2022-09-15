using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    
    [SerializeField]Transform orientation; 
    [SerializeField] float moveMultiplier;
    [SerializeField] float airMoveMultiplier;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance;
    public bool isGrounded;

    [Header("Movement")]
    public float moveSpeed;

    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
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
    private void Update() {
        ControlDrag();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
 
    public void MovePlayer(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        if(isGrounded)
        {
            moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else
        {
            moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;
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
}
