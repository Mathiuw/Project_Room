using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform[] elevatorDoors;
    
    public Transform[] elevatorPanels;

    public Material[] glows;

    private void Awake()
    {
        foreach (Transform panel in elevatorPanels)
        {
            if (panel.GetComponentInChildren<interactive>().interactiveTypes == interactive.InteractiveTypes.OpenElevator)            
                panel.GetComponentInChildren<Name>().SetText("Open Elevator");
            
            else if (panel.GetComponentInChildren<interactive>().interactiveTypes == interactive.InteractiveTypes.CloseElevatorAndEndGame)            
                panel.GetComponentInChildren<Name>().SetText("Close Elevator");           
        }
    }
}