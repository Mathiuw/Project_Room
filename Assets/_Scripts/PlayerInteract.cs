using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] LayerMask interactiveMask;
    [SerializeField] float rayLength = 5;
    Transform cameraTransform;

    void Start() => cameraTransform = Camera.main.transform;

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.E)) Interacting(transform);
    } 

    public void Interacting(Transform t) 
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, interactiveMask))
        {
            Interact interact;

            if ((interact = hit.transform.GetComponentInParent<Interact>()) && interact.enabled) 
            {
                interact.Interacting(t);
                Debug.Log("<b><color=magenta>" + transform.name + "</color></b> interacted with <b><color=cyan>" + interact.transform.name + "</color></b>");
            } 
        }
    }
}