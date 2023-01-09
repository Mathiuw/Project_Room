using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private LayerMask interactiveMask;
    [SerializeField] private float rayLength = 5;
    private Transform cameraTransform;
    private RaycastHit hit;

    void Start()
    {
        cameraTransform = Player.Instance.PlayerCamera;
    }

    public void Interacting() 
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, interactiveMask))
        {
            interactive interactive = hit.transform.GetComponentInParent<interactive>();

            if (interactive && interactive.enabled) interactive.Interact();
        }
    }
}
