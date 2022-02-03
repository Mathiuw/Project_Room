using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadNames : MonoBehaviour
{
    [SerializeField]private LayerMask layersToRead;
    [SerializeField]private TextMeshProUGUI displayText;
    [SerializeField]private Transform playerCamera;

    private RaycastHit hit;

    private void Awake()
    {
        playerCamera = GameObject.Find("Main Camera").transform;
    }

    private void Update()
    {
        if (Physics.Raycast(playerCamera.position,playerCamera.forward,out hit, 5f,layersToRead))
        {
            if (hit.transform.GetComponent<Name>().enabled)
            {
                displayText.SetText(hit.transform.GetComponent<Name>().text);
            }
        }
        else
        {
            displayText.SetText("");
        }
    }
}
