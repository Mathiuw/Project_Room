using UnityEngine;

public class PlayerCameraMove : MonoBehaviour
{
    [Header("Camera settings")]
    [Range(1,100)]
    public float sensibility;
    [SerializeField] float multiplier;

    float mouseX, mouseY;    
    float xRotation, yRotation;

    [Header("Rotate sideways")]
    [Range(1, 5)]
    [SerializeField] float angleLimit = 2;
    [SerializeField] float smooth = 20;

    public float angle { get; private set; }

    void Start() 
    {
        CursorState.CursorLock();
    }

    void Update() 
    {
        camMove();

        //Rotate sideways camera
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, RotateVector());
    } 

    public void SetSensiblility(float value) => sensibility = value;

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

    float RotateVector()
    {
        angle -= (Input.GetAxisRaw("Horizontal")) * smooth * Time.deltaTime;
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