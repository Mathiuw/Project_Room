using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Movement")]
    [Range(1, 100)]
    [SerializeField] float sensibility = 50;
    [SerializeField] float multiplier = 1;

    float mouseX, mouseY;
    float xRotation, yRotation;
    Transform desiredCameraPosition;

    [Header("Camera Roll")]
    [SerializeField] bool canRoll = true;
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
        // Search player
        Player player = FindFirstObjectByType<Player>();

        // Get the player camera position
        if (player != null)
        {
            desiredCameraPosition = player.GetCameraPivot();
        }
        else 
        {
            Debug.LogError("Cant find Player");
        }

        // Lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Move camera
        CameraMove();

        // Follows the player camera position
        transform.position = desiredCameraPosition.position;
    }

    void CameraMove() 
    {
        mouseX = Input.GetAxisRaw("Mouse X") * sensibility * multiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * sensibility * multiplier;

        //Vector3 rot = transform.rotation.eulerAngles;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -89, 89);

        if (canRoll)
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
