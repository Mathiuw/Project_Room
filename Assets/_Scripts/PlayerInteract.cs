using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField] LayerMask interactiveMask;
    [SerializeField] float rayLength = 5;
    Transform playerCamera;

    void Start()
    {
        // Find PlayerCamera
        PlayerCamera playerCameraComponent = FindFirstObjectByType<PlayerCamera>();

        if (playerCameraComponent != null)
        {
            playerCamera = playerCameraComponent.transform;
        }
        else
        {
            Debug.Log("Cant find PlayerCamera");
            enabled = false;
        }

        //input.Player.Interact.started += Interact;
    }

    public void Interact(InputAction.CallbackContext value)
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayLength, interactiveMask))
        {
            IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(transform);
            }
        }
    }
}
