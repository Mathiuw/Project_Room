using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] LayerMask interactiveMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] float rayLength = 5;
    Transform cameraTransform;

    void Start() => cameraTransform = Camera.main.transform;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.E))Interacting(transform);
    } 

    public void Interacting(Transform t) 
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, interactiveMask))
        {
            Interact interact;

            if ((interact = hit.transform.GetComponentInParent<Interact>()) && interact.enabled) interact.Interacting(t);
        }
    }
}
