using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    [Range(1, 100)]
    [SerializeField] float sensibility = 5;
    [SerializeField] float multiplier = 1;
    float mouseX, mouseY;
    float xRotation, yRotation;
    Transform cameraPivot;

    [Header("Camera Roll")]
    [SerializeField] bool cameraRoll = true;
    [Range(1, 5)]
    [SerializeField] float angleLimit = 2;
    [SerializeField] float smooth = 20;
    float angle;

    [Header("Weapon")]
    [SerializeField] Transform weaponHolder;

    public void SetSensibility(float sensibility) { this.sensibility = sensibility; } 
    public Transform GetWeaponHolder() { return weaponHolder; }

    void Start()
    {
        // Lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Search player
        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();

        // Get the player camera position
        if (playerMovement)
        {
            cameraPivot = playerMovement.GetCameraPivot();
        }
        else Debug.LogError("Cant find Player");
    }

    void Update()
    {
        // Follows the player camera position
        transform.position = cameraPivot.position;

        // Move camera
        CameraMove();
    }

    void CameraMove() 
    {
        mouseX = Input.GetAxisRaw("Mouse X") * sensibility * multiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * sensibility * multiplier;

        //Vector3 rot = transform.rotation.eulerAngles;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -89, 89);

        if (cameraRoll)
        {
            // Camera rotation with roll
            transform.rotation = Quaternion.Euler(xRotation, yRotation, CameraRollVector());
        }
        else 
        {
            // Camera rotation without roll
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }

    float CameraRollVector()
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
