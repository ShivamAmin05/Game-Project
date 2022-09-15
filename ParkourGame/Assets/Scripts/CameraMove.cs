using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    // Start is called before the first frame update
   
    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
