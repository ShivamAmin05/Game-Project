using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private WallRunning wallRun;
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
    public float desiredMoveSpeed;
    public float lastDesiredMoveSpeed;
    public float standSpeedTimeMultiplier;
    public float airSpeedTimeMultiplier;
    public float timeMultiplier;
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

    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveDirection = Vector3.zero;
        playerBody = GameObject.Find("PlayerBody");
        playerAnimator = playerBody.GetComponent<Animator>();
        wallRun = GetComponent<WallRunning>();
        desiredMoveSpeed = standSpeed;
    }
    private void ControlDrag()
    {   
        if(isGrounded && !playerAnimator.GetBool("isDashing"))
        {
            rb.drag = groundDrag;
        }
        else if(playerAnimator.GetBool("isWallRunningRight") || playerAnimator.GetBool("isWallRunningLeft"))
        {
            rb.drag = wallRun.wallRunDrag;
        }
        else if(!isGrounded)
        {
            rb.drag = airDrag;
        }
    }

    public void ControlSpeed()
    {
        if(onSlope() && !playerAnimator.GetBool("isJumping") && !playerAnimator.GetBool("isDashing"))
        {
            if(rb.velocity.magnitude > currSpeed)
            {
                rb.velocity = rb.velocity.normalized * currSpeed;
            }
        }
        else
        {
            Vector3 vel = new Vector3(rb.velocity.x,0f,rb.velocity.z);
            if(vel.magnitude > currSpeed)
            {
                Vector3 limitVel = vel.normalized * currSpeed;
                rb.velocity = new Vector3(limitVel.x,rb.velocity.y,limitVel.z);

            }
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
    public Vector3 getSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction,slopeHit.normal).normalized;
    }
    private void Update() {
        ControlDrag();
        ControlSpeed();
        rb.useGravity = !((moveDirection.x == 0 && moveDirection.z == 0) && onSlope());
        playerAnimator.SetFloat("currSpeed",currSpeed);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    
        playerAnimator.SetBool("isGrounded",isGrounded);

        if(Mathf.Abs(lastDesiredMoveSpeed - desiredMoveSpeed) > standSpeed - 1 && currSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(lerpMoveSpeed());
        }
        // else if(moveDirection.magnitude == 0)
        // {
        //     // desiredMoveSpeed = standSpeed;
        //     // timeMultiplier = 15;
        // }
        else
        {
            currSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;

    }   
    private IEnumerator lerpMoveSpeed()
    {
        float time = 0;
        float startSpeed = currSpeed;
        float diffMoveSpeed = Mathf.Abs(desiredMoveSpeed - currSpeed);

        while(time < diffMoveSpeed)
        {
            currSpeed = Mathf.Lerp(startSpeed,desiredMoveSpeed,time/diffMoveSpeed);
            time += (Time.deltaTime * timeMultiplier);
            yield return null;
        }

        currSpeed = desiredMoveSpeed;
    }
    public void MovePlayer(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;

        playerAnimator.SetBool("isMovingHorizontal", input.x == 0);
        playerAnimator.SetBool("isMovingVertical", input.y == 0);
        playerAnimator.SetFloat("horizontalSpeed",input.x);
        playerAnimator.SetFloat("verticalSpeed",input.y);

        if(moveDirection.x != 0 && moveDirection.z != 0)
        {
            playerAnimator.SetBool("isMoving", true);
            if(isGrounded && onSlope() && rb.velocity.y > -0.1f)
            {
                rb.AddForce(getSlopeMoveDirection(moveDirection) * currSpeed * slopeMultiplier * moveMultiplier, ForceMode.Force);
            }
            if(isGrounded && onSlope() && rb.velocity.y < -0.1f)
            {
                rb.AddForce(getSlopeMoveDirection(moveDirection) * currSpeed * slopeMultiplier * moveMultiplier, ForceMode.Force);
            }
            if(!isGrounded)
            {
                rb.AddForce(moveDirection.normalized * currSpeed * moveMultiplier * airMoveMultiplier, ForceMode.Force);
            }
            if(isGrounded && !onSlope())
            {
                rb.AddForce(moveDirection.normalized * currSpeed * moveMultiplier, ForceMode.Force);
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
            desiredMoveSpeed = standSpeed;
            rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetBool("isJumping", true);
        }
        else
        {
            playerAnimator.SetBool("isJumping", false);    
        }
    }
}
