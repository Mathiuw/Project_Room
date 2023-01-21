using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float senX, senY;
    [SerializeField] float multiplier;

    float mouseX, mouseY;    
    float xRotation, yRotation;

    void Start()
    {
        CursorState.CursorLock();
    }

    void Update() => camMove();

    void camMove()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * senX * multiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * senY * multiplier;

        Vector3 rot = transform.rotation.eulerAngles;

        yRotation = rot.y + mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -89, 89);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}