using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour,ICanDo
{
    private bool canDo = true;

    [SerializeField] private LayerMask interactiveMask;
    [SerializeField] private float rayLength = 5;
    private Transform cameraTransform;
    private RaycastHit hit;

    void Awake()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    void Update()
    {
        if (canDo)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, interactiveMask))
                {
                    interactive interactive = hit.transform.GetComponentInParent<interactive>();

                    if (interactive && interactive.enabled)
                    {
                        interactive.Interact();
                    }
                }
            }
        }
    }

    public void CheckIfCanDo(bool check)
    {
        if (check)
        {
            canDo = false;
        }
        else canDo = true;
    }
}
