using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    
    [SerializeField]Transform orientation; 

    [Header("Movement")]
    public float moveSpeed = 5f;

 

    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }
    private void Update() {
     
    }
 
    public void MovePlayer(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = orientation.forward * moveDirection.z + orientation.right * moveDirection.x;
        rb.AddForce(moveDirection.normalized * moveSpeed * 1, ForceMode.Acceleration);

    }
}
