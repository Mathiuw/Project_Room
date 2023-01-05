using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateSideways : MonoBehaviour, ICanDo
{
    bool canDo = true;
    [Range(1,5)]
    [SerializeField] float angleLimit;
    [Range(0.05f,0.25f)]
    [SerializeField] float smooth;

    [Header("Angle")]
    [SerializeField]float angle;

    Transform cameraTransform;

    void Awake()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;

        FindObjectOfType<Pause>().Paused += CheckIfCanDo;
    }

    void Update()
    {
        if (!canDo) return;
        cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, RotateVector());
    }

    float RotateVector()
    {
        if (Input.GetKey(KeyCode.A)) angle += smooth;
        else if (Input.GetKey(KeyCode.D)) angle -= smooth;
        else
        {
            if (angle > 0f) angle -= smooth;
            else if (angle < 0f) angle += smooth;
        }
        if (angle > angleLimit) angle = angleLimit;
        else if (angle < -angleLimit) angle = -angleLimit;
        return angle;
    }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}
