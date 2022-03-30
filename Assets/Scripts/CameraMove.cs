using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour,ICanDo
{
    private bool canDo = true;
    private Camera cam;

    [Header("Camera Settings")]
    public float senX, senY;
    public float multiplier;
    private float mouseX, mouseY;    
    private float xRotation, yRotation;

    private void Awake()
    {
        CursorState.CursorLock();

        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    void Update()
    {
        if (!canDo)return;
        camMove();
    }

    void FixedUpdate()
    {
        if (!canDo)return;
        bodyRot();
    }

    void camMove()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * senX * multiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * senY * multiplier;

        Vector3 rot = cam.transform.rotation.eulerAngles;

        yRotation = rot.y + mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void bodyRot()
    {
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}