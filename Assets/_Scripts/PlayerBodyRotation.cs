using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyRotation : MonoBehaviour
{
    Transform cameraTransform;

    void Start() 
    {
        cameraTransform = Player.Instance.transform.Find("Camera");
    }

    void FixedUpdate() 
    {
        BodyRotation();
    }

    void BodyRotation() 
    {
        transform.localRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }
}
