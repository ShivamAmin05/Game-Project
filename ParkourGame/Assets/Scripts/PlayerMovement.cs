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
    public float standHeight;
    public float standRadius;
    public float crouchHeight;
    
    [Header("Movement")]
    public float currSpeed;
    public float standSpeed;
    public float crouchSpeed;
    [SerializeField] float moveMultiplier;
    [SerializeField] float slopeMultiplier;
    [SerializeField] float airMoveMultiplier;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float groundDistance;
    public Vector3 moveDirection;
    public bool isGrounded;
    [Header("Slopes")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    // [Header("Animation")]
    // // public float animationAcel;
    // // public float animationDecel;
    // public float horizontalSpeed;
    // public float verticalSpeed;

    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveDirection = Vector3.zero;
        playerBody = GameObject.Find("PlayerBody");
        playerAnimator = playerBody.GetComponent<Animator>();
        currSpeed = standSpeed;
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
    
    public bool onSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down,out slopeHit, 1.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return (angle < maxSlopeAngle && angle != 0);
        }
        return false;
     }
    public Vector3 getSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection,slopeHit.normal).normalized;
    }
    private void Update() {
        ControlDrag();
        rb.useGravity = !((moveDirection.x == 0 && moveDirection.z == 0) && onSlope());
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
    // public void animationControl(float x)
    // {
    //     if(x == 1 && horizontalSpeed < 10)
    //     {
    //         horizontalSpeed += Time.deltaTime * animationAcel;
    //     }
    //     if(x == -1 && horizontalSpeed > -10)
    //     {
    //         horizontalSpeed -= Time.deltaTime * animationAcel;
    //     }
    //     if(x == 0 && horizontalSpeed > 0)
    //     {
    //         horizontalSpeed -= Time.deltaTime * animationDecel;
    //     }
    //     if(x == 0 && horizontalSpeed < 0)
    //     {
    //         horizontalSpeed += Time.deltaTime * animationDecel;
    //     }
    // }
    public void MovePlayer(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;

        if(input.x == 0)
        {
            playerAnimator.SetBool("isMovingHorizontal", false);
        }
        else
        {
            playerAnimator.SetBool("isMovingHorizontal", true);
        }
        if(input.y == 0)
        {
            playerAnimator.SetBool("isMovingVertical", false);
        }
        else
        {
            playerAnimator.SetBool("isMovingVertical", true);
        }

        playerAnimator.SetFloat("horizontalSpeed",input.x);
        playerAnimator.SetFloat("verticalSpeed",input.y);
        playerAnimator.SetFloat("verticalSpeed",input.y);

        if(moveDirection.x != 0 && moveDirection.z != 0)
        {
            playerAnimator.SetBool("isMoving", true);
            if(isGrounded && onSlope())
            {
                rb.AddForce(getSlopeMoveDirection() * currSpeed * slopeMultiplier * moveMultiplier, ForceMode.Force);
            }
            if(isGrounded && !onSlope())
            {
                rb.AddForce(moveDirection.normalized * currSpeed * moveMultiplier, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * currSpeed * moveMultiplier * airMoveMultiplier, ForceMode.Force);
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
