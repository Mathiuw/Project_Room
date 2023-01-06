using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CameraRotateSideways : MonoBehaviour
{
    [Range(1,5)]
    [SerializeField] float angleLimit;   
    [SerializeField] float smooth;

    [Header("Angle")]
    [SerializeField]float angle;

    [SerializeField]Transform cameraTransform;
    Player player;

    void Start()
    {
        player = Player.Instance;

        cameraTransform = player.GetComponentInChildren<CinemachineVirtualCamera>().transform;
    }

    void Update()
    {
        cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, RotateVector());
    }

    float RotateVector()
    {
        angle += -(Input.GetAxisRaw("Horizontal")) * smooth * Time.deltaTime;
        angle = Mathf.Clamp(angle, -angleLimit, angleLimit);

        if (Input.GetAxisRaw("Horizontal") != 0) return angle;
         
        if (angle > 0f) 
        {
            angle -= smooth * Time.deltaTime;
            angle = Mathf.Clamp(angle, 0f, angleLimit);
        } 
        else if (angle < 0f) 
        {
            angle += smooth * Time.deltaTime;
            angle = Mathf.Clamp(angle, -angleLimit, 0f);
        } 
        return angle;
    }
}
