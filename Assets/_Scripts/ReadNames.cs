using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadNames : MonoBehaviour
{
    [SerializeField] LayerMask layersToRead;
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] Transform playerCamera;

    private void Awake()
    {
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        RaycastHit hit;

        displayText.SetText("");

        if (Physics.Raycast(playerCamera.position,playerCamera.forward,out hit, 5f,layersToRead))
        {
            Name name;
            if ((name = hit.transform.GetComponentInParent<Name>()) && name.enabled) displayText.SetText(name.text);
        }
    }
}
