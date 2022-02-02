using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform[] elevatorDoors;

    [SerializeField] private Transform[] elevatorPanels; 

    private void Awake()
    {
        foreach (Transform panel in elevatorPanels)
        {
            if (panel.GetComponent<interactive>().interactiveTypes == interactive.InteractiveTypes.OpenElevator)
            {
                panel.GetComponent<Name>().text = "Open Elevator";
            }
            else if (panel.GetComponent<interactive>().interactiveTypes == interactive.InteractiveTypes.CloseElevatorAndSwitchLevel)
            {
                panel.GetComponent<Name>().text = "Close Elevator";
            }
        }
    }
}