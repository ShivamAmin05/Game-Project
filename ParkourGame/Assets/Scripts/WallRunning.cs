
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
    private GameObject playerBody;
    [Header("Wall Running")]
    [SerializeField] float wallDistance;
    [SerializeField] float minimumJumpHeight;
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    private bool wallLeft;
    private bool wallRight;
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
    // Animator playerAnimator;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        move = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        dash = GetComponent<DashMovement>();
    }
  
    public void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, - orientation.right,out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right,out rightWallHit, wallDistance);
        return;
    }

    void Update() 
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
                StopWallRun();
            }
        }
        if(move.isGrounded)
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
            // playerAnimator.SetBool("isWallRun", true);
            // playerBody.transform.rotation = Quaternion.Euler(45f,0f, 0f);
            // charTilt = 25;
        }
        if (wallRight)
        {
            tilt = Mathf.Lerp(tilt,camTilt,camTiltTime * Time.deltaTime);
            // playerAnimator.SetBool("isWallRun", true);
            // charTilt = 25;
        }
       
        
    }
    public void WallJump()
    {
        if(!move.isGrounded)
        {
            if(wallLeft)
            {
                Vector3 wallRunJumpDistance = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce (wallRunJumpDistance * wallRunJumpForce * 100,ForceMode.Force);
            }
            else if(wallRight)
            {
                Vector3 wallRunJumpDistance = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce (wallRunJumpDistance * wallRunJumpForce * 100,ForceMode.Force);
            }
        }
    }
    public void StopWallRun()
    {
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, look.fov, FovTransition * Time.deltaTime);
        tilt = Mathf.Lerp(tilt,0,camTiltTime * Time.deltaTime);
        // playerAnimator.SetBool("isWallRun", false);
        // charTilt = 0;
    }

}
