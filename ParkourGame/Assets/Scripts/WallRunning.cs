using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerMovement move;
    private DashMovement dash;
    private PlayerLook look;
    [Header("References")]
    [SerializeField] Transform orientation;
    Animator playerAnimator;
    private GameObject playerBody;
    [Header("Wall Running")]
    [SerializeField] float wallDistance;
    [SerializeField] float minimumJumpHeight;
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    [SerializeField] float wallRunSpeed;
    [SerializeField] float wallRunTimeAcel;
    public float wallRunDrag;
    public bool wallLeft;
    public bool wallRight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float wallFov;
    [SerializeField] float FovTransition;
    [SerializeField] float camTilt;
    // public float charTilt;
    [SerializeField] float camTiltTime;
    public float tilt {get; private set; }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        move = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        dash = GetComponent<DashMovement>();
        playerBody = GameObject.Find("PlayerBody");
        playerAnimator = playerBody.GetComponent<Animator>();
    }
  
    public void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, - orientation.right,out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right,out rightWallHit, wallDistance);
        return;
    }

    private void FixedUpdate()
    {
        CheckWall();
        if(!move.isGrounded)
        {
            if (wallLeft || wallRight)
            {
                StartWallRun();
                dash.dashNum = 0;
            }
            else
            {
                Invoke("StopWallRun",0.1f);
            }
        }
        if(move.isGrounded && !move.onSlope())
        {
            StopWallRun();
        }
        }
    public void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallFov, FovTransition * Time.deltaTime);
        if(wallLeft)
        {
            tilt = Mathf.Lerp(tilt,-camTilt,camTiltTime * Time.deltaTime);
            playerAnimator.SetBool("isWallRunningLeft", true);
            move.desiredMoveSpeed = wallRunSpeed;
            move.timeMultiplier = wallRunTimeAcel;
        }
        if (wallRight)
        {
            tilt = Mathf.Lerp(tilt,camTilt,camTiltTime * Time.deltaTime);
            playerAnimator.SetBool("isWallRunningRight", true);
            move.desiredMoveSpeed = wallRunSpeed;
            move.timeMultiplier = wallRunTimeAcel;
        }
    }
    public void WallJump()
    {
        if(!move.isGrounded)
        {
            // move.desiredMoveSpeed = move.standSpeed;
            if(wallLeft && !move.onSlope())
            {
                Vector3 wallRunJumpDistance = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce (wallRunJumpDistance * wallRunJumpForce * 100,ForceMode.Force);
                move.timeMultiplier = move.airSpeedTimeMultiplier;
            }
            else if(wallRight && !move.onSlope())
            {
                Vector3 wallRunJumpDistance = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce (wallRunJumpDistance * wallRunJumpForce * 100,ForceMode.Force);
                move.timeMultiplier = move.airSpeedTimeMultiplier;
            }
        }
    }
    public void StopWallRun()
    {
        if(!move.onSlope() && move.currSpeed > move.standSpeed)
        {
            move.desiredMoveSpeed = move.standSpeed;
            move.timeMultiplier= move.airSpeedTimeMultiplier;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, look.fov, FovTransition * Time.deltaTime);
        tilt = Mathf.Lerp(tilt,0,camTiltTime * Time.deltaTime);
        playerAnimator.SetBool("isWallRunningLeft", false);
        playerAnimator.SetBool("isWallRunningRight", false);
    }
}
