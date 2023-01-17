using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] float senX, senY;
    [SerializeField] float multiplier;
    float mouseX, mouseY;    
    float xRotation, yRotation;
    Transform cam;

    private void Start()
    {
        CursorState.CursorLock();

        cam = Player.Instance.transform.Find("Camera");
    }

    public void camMove()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * senX * multiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * senY * multiplier;

        Vector3 rot = cam.transform.rotation.eulerAngles;

        yRotation = rot.y + mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -89, 89);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void bodyRot() => transform.localRotation = Quaternion.Euler(0, yRotation, 0);
}