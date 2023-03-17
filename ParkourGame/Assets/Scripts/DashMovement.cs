using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMovement : MonoBehaviour
{
    public Transform orientation;
    public Transform camPos;
    public Camera playerCam;
    private Rigidbody rb;
    private PlayerMovement move;

    [Header("Dash")]
    public float dashHeight;
    public float dashRadius;
    public float slideSpeed;
    [SerializeField] float dashForce;
    [SerializeField] float groundDashMultiplier;
    [SerializeField] float dashUpwardForce;
    [SerializeField] int setDashes;
    [SerializeField] float slideTimer;
    [SerializeField] float slideTimeMultiplier;
    public int dashNum;

    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float dashFov;

    private Vector3 direction = Vector2.zero;
    Animator playerAnimator;
    CapsuleCollider hitBox;
    GameObject playerBody;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        move = GetComponent<PlayerMovement>();
        playerBody = GameObject.Find("PlayerBody");
        playerAnimator = playerBody.GetComponent<Animator>();
        hitBox = playerBody.GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        direction = getDirection();
        if(slideTimer > 0 && playerAnimator.GetBool("isDashing"))
        {
            slideTimer -= Time.deltaTime;
        }
        if(slideTimer <= 0 && playerAnimator.GetBool("isDashing") && !move.onSlope())
        {
            move.desiredMoveSpeed = move.standSpeed;
        }
        if(move.isGrounded)
        {
            dashNum = 0;
        }
    }
        
    public void Dash()
    { 
        if(!move.isGrounded)
        {
            // divided dashNum by 3 because every time dash is called unity increments dashNum by 3
            if(dashNum/3 < setDashes)
            {
                cam.fieldOfView = dashFov;
                Vector3 dashingForce = direction * dashForce + orientation.up * dashUpwardForce;
                rb.AddForce(dashingForce, ForceMode.Impulse);
                dashNum++;
                playerAnimator.SetBool("isAirDashing", true);
                Invoke("ResetDash", 1f);
            }
        }
        else
        {
            startSlide();
        }
    }

    public void startSlide()
    {
        slideTimer = 2;
        move.timeMultiplier = slideTimeMultiplier;
        move.desiredMoveSpeed = 0;
        cam.fieldOfView = dashFov;
        Vector3 dashingForce = direction * dashForce * groundDashMultiplier;
        rb.AddForce(dashingForce, ForceMode.Impulse);
        playerAnimator.SetBool("isDashing", true);
        hitBox.height = dashHeight;
        hitBox.radius = dashRadius;
        hitBox.center = new Vector3(0f,0.2f,0f);
        // if(slideTimer == 0)
        // {
        // Invoke("ResetDash", 1.4f);
        if(move.onSlope() && rb.velocity.y < -0.1f)
        {
            move.desiredMoveSpeed = slideSpeed;
        }
        
        if(!move.onSlope() && move.currSpeed > move.standSpeed)
        {
            move.desiredMoveSpeed = move.standSpeed;
        }

    }

    public void ResetDash()
    {
        playerAnimator.SetBool("isDashing", false);
        playerAnimator.SetBool("isAirDashing", false);
        hitBox.height = move.standHeight;
        hitBox.radius = move.standRadius;
        hitBox.center = new Vector3(0f,0.8f,0f);
        move.timeMultiplier = move.standSpeedTimeMultiplier;
        move.desiredMoveSpeed = move.standSpeed;
    }

    public Vector3 getDirection()
    {
        Vector3 direction = new Vector3();
        if(move.isGrounded && !move.onSlope())
        {
            direction = move.moveDirection;
        }
        else if(move.isGrounded && move.onSlope())
        {
            direction = move.getSlopeMoveDirection(move.moveDirection);
        }
        else
        {
            direction = camPos.forward;
        }
        return direction.normalized;
    }
}
