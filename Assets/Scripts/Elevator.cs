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
            if (panel.GetComponentInChildren<interactive>().interactiveTypes == interactive.InteractiveTypes.OpenElevator)
            {
                panel.GetComponentInChildren<Name>().text = "Open Elevator";
            }
            else if (panel.GetComponentInChildren<interactive>().interactiveTypes == interactive.InteractiveTypes.CloseElevatorAndSwitchLevel)
            {
                panel.GetComponentInChildren<Name>().text = "Close Elevator";
            }
        }
    }
}