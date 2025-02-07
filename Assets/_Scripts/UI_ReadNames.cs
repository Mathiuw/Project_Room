using UnityEngine;
using TMPro;

public class UI_ReadNames : MonoBehaviour
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
            ShowNameToHUD showNameToHUD;
            if ((showNameToHUD = hit.transform.GetComponentInParent<ShowNameToHUD>()) && showNameToHUD.enabled) displayText.SetText(showNameToHUD.GetText());
        }
    }
}
