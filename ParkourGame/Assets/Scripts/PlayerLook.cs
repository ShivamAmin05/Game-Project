using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float xSens;
    [SerializeField] float ySens;
    public Transform orientation;
    public Camera playerCam;
    float xRotation;
    float yRotation;
    float camMultiplier = 0.1f;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void camMovement(Vector2 input)
    {
        float xInput = input.x;
        float yInput = input.y;

        yRotation += xInput * xSens * camMultiplier;
        xRotation -= yInput * ySens * camMultiplier;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
