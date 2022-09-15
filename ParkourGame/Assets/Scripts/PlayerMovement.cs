using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    
    [SerializeField]Transform orientation; 
<<<<<<< HEAD
=======
    [SerializeField]float moveMultiplier;
    [SerializeField]float airMoveMultiplier;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
>>>>>>> parent of cfd164a2 (Push)

    [Header("Movement")]
    public float moveSpeed = 5f;

 

    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }
<<<<<<< HEAD
    private void Update() {
     
=======
    private void ControlDrag()
    {        
        rb.drag = groundDrag;
    }
    private void Update() {
        ControlDrag();
>>>>>>> parent of cfd164a2 (Push)
    }
 
    public void MovePlayer(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;
<<<<<<< HEAD
        rb.AddForce(moveDirection.normalized * moveSpeed * 1, ForceMode.Acceleration);
=======
        rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
>>>>>>> parent of cfd164a2 (Push)

    }
}
