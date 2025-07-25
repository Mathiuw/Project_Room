using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField] LayerMask interactiveMask;
    [SerializeField] float rayLength = 5;
    PlayerMovement playerMovement;
    Transform playerCamera;

    void Start()
    {
        // Find PlayerCamera
        CameraMovement cameraMovement = FindFirstObjectByType<CameraMovement>();

        if (cameraMovement != null)
        {
            playerCamera = cameraMovement.transform;
        }
        else
        {
            Debug.Log("Cant find PlayerCamera");
            enabled = false;
        }

        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement)
        {
            playerMovement.Input.Player.Interact.started += Interact;
        }
    }

    private void OnDisable()
    {
        playerMovement.Input.Player.Interact.started -= Interact;
    }

    public void Interact(InputAction.CallbackContext value)
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayLength, interactiveMask))
        {
            IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            if (interactable != null) interactable.Interact(transform);

            Debug.DrawLine(playerCamera.position, hit.point, Color.green, 1f);
        }
        else
        {
            Debug.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * rayLength, Color.red, 1f);
        }
    }
}
