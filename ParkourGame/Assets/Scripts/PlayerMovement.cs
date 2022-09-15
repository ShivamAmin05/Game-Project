using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    
    [SerializeField]Transform orientation; 
    [SerializeField]float moveMultiplier;
    [SerializeField]float airMoveMultiplier;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    [Header("Movement")]
    public float moveSpeed = 5f;

 

    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }
    private void ControlDrag()
    {        
        rb.drag = groundDrag;
    }
    private void Update() {
        ControlDrag();
    }
 
    public void MovePlayer(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;
        rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);

    }
}
