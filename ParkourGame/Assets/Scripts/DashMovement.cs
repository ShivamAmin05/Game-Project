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

    [SerializeField] float dashForce;
    [SerializeField] float groundDashMultiplier;
    [SerializeField] float dashUpwardForce;
    // [SerializeField] float dashDuration;
    [SerializeField] int setDashes;
    private int dashNum;

    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float dashFov;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    public void Dash()
    {
        Transform forwardT;
        
        forwardT = camPos;
        
        Vector3 direction = getDirection(forwardT);
        if(movement.isGrounded == false)
        {
            // diveded dashNum by 3 because every time dash is called unity increments dashNum by 3
            if(dashNum/3 < setDashes)
            {
                cam.fieldOfView = dashFov;
                Vector3 dashingForce = direction * dashForce + orientation.up * dashUpwardForce;
                rb.AddForce(dashingForce, ForceMode.Impulse);
                dashNum++;
            }
        }
        else
        {
            cam.fieldOfView = dashFov;
            Vector3 dashingForce = orientation.forward * dashForce * groundDashMultiplier;
            rb.AddForce(dashingForce, ForceMode.Impulse);
        }
        
    }
    public Vector3 getDirection(Transform forwardT)
    {
        
        Vector3 direction = new Vector3();
        if(movement.moveDirection.z == 0 && movement.moveDirection.x == 0)
        {
            direction = forwardT.forward;
        }
        else
        {
            direction = forwardT.forward * movement.moveDirection.z + forwardT.right * movement.moveDirection.x;
        }
        return direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.isGrounded)
        {
            dashNum = 0;
        }
    }   
}
