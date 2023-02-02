using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerReadNames : MonoBehaviour
{
    [SerializeField] LayerMask layersToRead;
    [SerializeField] TextMeshProUGUI displayText;
    Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        RaycastHit hit;

        displayText.SetText("");

        if (Physics.Raycast(cameraTransform.position,cameraTransform.forward,out hit, 5f,layersToRead))
        {
            Name name;
            if ((name = hit.transform.GetComponentInParent<Name>()) && name.enabled) displayText.SetText(name.text);
        }
    }
}
