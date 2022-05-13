using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateSideways : MonoBehaviour
{
    [SerializeField] float angleLimit;
    [SerializeField] float smooth;
    [Header("Angle")]
    [SerializeField]float angle;

    GameObject playerCamera;
    Rigidbody rb;

    void Awake()
    {
        rb = transform.root.GetComponentInChildren<Rigidbody>();
        playerCamera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        Transform cameraTransform = playerCamera.transform;

        cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, RotateVector());
    }

    float RotateVector()
    {
        if (Input.GetKey(KeyCode.A)) angle += smooth;    
        else if (Input.GetKey(KeyCode.D)) angle -= smooth;
        else
        {
            if (angle > 0) angle -= smooth;
            else if (angle < 0) angle += smooth;
        }

        if (angle > angleLimit) angle = angleLimit;
        else if (angle < -angleLimit) angle = -angleLimit;

        return angle;
    }
}
