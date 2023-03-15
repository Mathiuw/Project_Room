using UnityEngine;

public class PlayerCameraMove : MonoBehaviour
{
    public static PlayerCameraMove instance;

    [Range(1,100)]
    public float sensibility;
    [SerializeField] float multiplier;

    float mouseX, mouseY;    
    float xRotation, yRotation;

    void Awake() => instance = this;

    void Start() => CursorState.CursorLock();   

    void Update() => camMove();

    void camMove()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * sensibility * multiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * sensibility * multiplier;

        Vector3 rot = transform.rotation.eulerAngles;

        yRotation = rot.y + mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -89, 89);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}