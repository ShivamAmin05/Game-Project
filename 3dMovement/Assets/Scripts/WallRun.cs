
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerMovement move;
    [SerializeField] Transform orientation;
    [Header("Wall Running")]
    [SerializeField] float wallDistance;
    [SerializeField] float minimumJumpHeight;
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    bool wallLeft = false;
    bool wallRight = false;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float fov;
    [SerializeField] float wallFov;
    [SerializeField] float wallFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;
    public float tilt {get; private set; }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        move = GetComponent<PlayerMovement>();
    }
    public bool CanWallRun()
    {
        //returns false if the player is grounded
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
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

        if(CanWallRun() && !move.isGrounded)
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
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
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallFov, wallFovTime * Time.deltaTime);
        if(wallLeft)
        {
            tilt = Mathf.Lerp(tilt,-camTilt,camTiltTime * Time.deltaTime);
        }
        if (wallRight)
        {
            tilt = Mathf.Lerp(tilt,camTilt,camTiltTime * Time.deltaTime);
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
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt,0,camTiltTime * Time.deltaTime);
    }

}
