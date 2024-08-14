using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] LayerMask interactiveMask;
    [SerializeField] float rayLength = 5;
    Transform cameraTransform;

    void Start()
    {
        Player player = GetComponent<Player>();    

        cameraTransform = player.GetPlayerCamera().transform;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.E)) Interact();
    } 

    public void Interact() 
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, interactiveMask))
        {
            IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            if (interactable != null) 
            {
                interactable.Interact(transform);
            } 
        }
    }
}