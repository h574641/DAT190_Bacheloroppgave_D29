using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float moveSpeed = 0.15f;

    float sensitivity = 3f;
    float horizontalInput;
    float verticalInput;
    float rotateHorizontal;
    float rotateVertical;
    float Horizontal;
    float Vertical;

    void Update () {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        rotateHorizontal = Input.GetAxis("Mouse X");
        rotateVertical = Input.GetAxis("Mouse Y");
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || verticalInput != 0) {
            transform.Translate(moveSpeed * transform.right * horizontalInput, Space.World);
            transform.Translate(moveSpeed * transform.forward * verticalInput, Space.World);
        }
        Horizontal += rotateHorizontal * sensitivity;
        Vertical += -rotateVertical * sensitivity;
        transform.rotation = Quaternion.Euler(Vertical,Horizontal, 0);
    }
}