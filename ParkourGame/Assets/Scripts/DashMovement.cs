using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMovement : MonoBehaviour
{
    public Transform orientation;
    public Camera playerCam;
    private Rigidbody rb;
    private PlayerMovement movement;

    [SerializeField] float dashForce;
    [SerializeField] float groundDashMultiplier;
    [SerializeField] float dashUpwardForce;
    // [SerializeField] float dashDuration;
    [SerializeField] int setDashes;
    private int dashNum;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    public void Dash()
    {
        if(movement.isGrounded == false)
        {
            if(dashNum/3 < setDashes)
            {
                Vector3 dashingForce = orientation.forward * dashForce + orientation.up * dashUpwardForce;
                rb.AddForce(dashingForce, ForceMode.Impulse);
                dashNum++;
            }
        }
        else
        {
            Vector3 dashingForce = orientation.forward * dashForce * groundDashMultiplier;
            rb.AddForce(dashingForce, ForceMode.Impulse);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.isGrounded)
        {
            dashNum = 0;
        }
        // print(dashNum);
    }   
}
