using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMovement : MonoBehaviour
{
    public Transform orientation;
    public Transform camPos;
    public Camera playerCam;
    private Rigidbody rb;
    private PlayerMovement movement;

    [Header("Dash")]
    public float dashHeight;
    public float dashRadius;
    [SerializeField] float dashForce;
    [SerializeField] float groundDashMultiplier;
    [SerializeField] float dashUpwardForce;
    [SerializeField] int setDashes;
    [SerializeField] float slideTimer;
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
        movement = GetComponent<PlayerMovement>();
        playerBody = GameObject.Find("PlayerBody");
        playerAnimator = playerBody.GetComponent<Animator>();
        hitBox = playerBody.GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        direction = getDirection();
        if(movement.isGrounded)
        {
            dashNum = 0;
        }
    }   

    public void Dash()
    { 
        if(movement.isGrounded == false)
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
        // slideTimer -= Time.deltaTime;
        cam.fieldOfView = dashFov;
        Vector3 dashingForce = direction * dashForce * groundDashMultiplier;
        rb.AddForce(dashingForce, ForceMode.Impulse);
        playerAnimator.SetBool("isDashing", true);
        hitBox.height = dashHeight;
        hitBox.radius = dashRadius;
        hitBox.center = new Vector3(0f,0.2f,0f);
        // if(slideTimer == 0)
        // {
        // movement.moveSpeed *= slideTimer;
        Invoke("ResetDash", 1.4f);
        // }
    }
    public void ResetDash()
    {
        playerAnimator.SetBool("isDashing", false);
        playerAnimator.SetBool("isAirDashing", false);
        hitBox.height = movement.standHeight;
        hitBox.radius = movement.standRadius;
        hitBox.center = new Vector3(0f,0.8f,0f);
    }

    public Vector3 getDirection()
    {
        Vector3 direction = new Vector3();
        if(movement.isGrounded && !movement.onSlope())
        {
            direction = movement.moveDirection;
        }
        else if(movement.isGrounded && movement.onSlope())
        {
            direction = movement.getSlopeMoveDirection();
        }
        else
        {
            direction = camPos.forward;
        }
        return direction.normalized;
    }

    // Update is called once per frame
    // void OnEnable()
    // {
    //     playerControls.Enable();
    // }
    // void OnDisable()
    // {
    //     playerControls.Disable()
    // }
}
