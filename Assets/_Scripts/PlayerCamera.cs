using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DynamicDOF))]
[RequireComponent(typeof(CameraShake))]
[RequireComponent(typeof(PlayerCameraAnimationManager))]
public class PlayerCamera : MonoBehaviour
{
    [Header("Camera movement")]
    [Range(1, 100)]
    [SerializeField] float sensibility = 50;
    [SerializeField] float multiplier = 1;

    float mouseX, mouseY;
    float xRotation, yRotation;
    Transform desiredCameraPosition;

    [Header("Rotate sideways")]
    [SerializeField] bool canRotate = true;
    [Range(1, 5)]
    [SerializeField] float angleLimit = 2;
    [SerializeField] float smooth = 20;
    float angle;

    [Header("Weapon")]
    [SerializeField] Transform weaponHolder;

    public void SetSensibility(float sensibility) { this.sensibility = sensibility; } 
    public Transform GetWeaponHolder() { return weaponHolder; }

    void Awake()
    {   
        // Lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Search player
        Player player = FindObjectOfType<Player>();

        // Get the player camera position
        if (player != null)
        {
            desiredCameraPosition = player.GetCameraDesiredPosition();
        }
        else 
        {
            Debug.Log("Cant find Player");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move camera
        CameraMove();

        // Follows the player camera position
        transform.position = desiredCameraPosition.position;

        // Rotate camera sideways
        if (canRotate) 
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, RotateCameraVector());
        }
    }

    void CameraMove() 
    {
        mouseX = Input.GetAxisRaw("Mouse X") * sensibility * multiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * sensibility * multiplier;

        Vector3 rot = transform.rotation.eulerAngles;

        yRotation = rot.y + mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -89, 89);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    float RotateCameraVector() 
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
