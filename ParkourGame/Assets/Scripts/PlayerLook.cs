using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    WallRunning wallRun;
    [SerializeField] float xSens;
    [SerializeField] float ySens;
    public Transform orientation;
    public Camera playerCam;
    public Transform Player;
    public Transform playerBody;
    public Transform head;
    float xRotation;
    float yRotation;
    float camMultiplier = 0.1f;
    public float fov;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        wallRun = GetComponent<WallRunning>();
    }
    public void camMovement(Vector2 input)
    {
        float xInput = input.x;
        float yInput = input.y;
        
        yRotation += xInput * xSens * camMultiplier;
        xRotation -= yInput * ySens * camMultiplier;
        // prevents the player from looking more than 90 degrees up or down
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);
        
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        head.transform.localRotation = Quaternion.Euler(xRotation,0,0);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);

        // Player.transform.rotation = Quaternion.Euler(xRotation,0f,0f);
        playerBody.transform.rotation = Quaternion.Euler(0f,yRotation, 0f);
        // playerBody.Rotate(orientation.up * xInput);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
