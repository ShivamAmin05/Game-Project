using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    WallRun wallRun;
    [SerializeField] Transform orientation;
    [Header("Camera")]
    public Camera cam;
    float xRotation;
    float yRotation;
    [SerializeField] float xSensitivity;
    [SerializeField] float ySensitivity;
    float camMultiplier = 0.01f;

    private void Start() {
        // locks teh cursor the screen and makes it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        wallRun = GetComponent<WallRun>();
        
    }
    public void ProcessLook(Vector2 input)
    {
        //gets the horizontal and vertical input fromm your mouse
        float mouseX = input.x;
        float mouseY = input.y;
        // yRotation(left and right) is obtained by adding the horizontal mouse input to itself
        yRotation += mouseX * xSensitivity * camMultiplier;
        //? xRotation(up and down) is obtained by subtracing the vertical mouse input from itself
        xRotation -= mouseY * ySensitivity * camMultiplier;
        // prevents the player from looking more than 180 vertically
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //? What is a Quaternian?
        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);
    }
}
