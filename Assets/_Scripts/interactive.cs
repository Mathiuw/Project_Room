using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactive : MonoBehaviour
{
    public InteractiveTypes interactiveTypes;

    [Header("KeyCard Reader")]
    public Items necessaryItem;

    public enum InteractiveTypes
    {
        OpenElevator,
        CloseElevatorAndEndGame,
    }

    public void Interact()
    {
        switch (interactiveTypes)
        {
            case InteractiveTypes.OpenElevator:
                OpenElevator();
                break;

            case InteractiveTypes.CloseElevatorAndEndGame:
                StartCoroutine(CloseElevatorAndEndGame());
                break;
        }
    }

    public void OpenElevator()
    {
        Animator elevatorAnimator = GetComponentInParent<Animator>();
        Elevator elevatorScript = GetComponentInParent<Elevator>();
        MeshRenderer panelMesh = elevatorScript.elevatorPanels[0].GetComponentInChildren<MeshRenderer>();
        Material[] changeGlows = new Material[panelMesh.materials.Length];

        for (int i = 0; i < panelMesh.materials.Length; i++)
        {
            if (i == 1)
            {
                changeGlows[i] = elevatorScript.glows[1];
            }
            else
            {
                changeGlows[i] = panelMesh.materials[i];
            }
        }

        panelMesh.materials = changeGlows;
        elevatorAnimator.SetTrigger("Open Door");
        elevatorAnimator.ResetTrigger("Close Door");
        elevatorAnimator.Play("Elevator_ExteriorDownButtom");
        transform.GetComponent<Name>().SetText("");
    }

    IEnumerator CloseElevatorAndEndGame()
    {
        Animator elevatorAnimator = GetComponentInParent<Animator>();
        Animator playerAnimator = Player.Instance.GetComponent<Animator>();
        Elevator elevatorScript = GetComponentInParent<Elevator>();
        MeshRenderer panelMesh = elevatorScript.elevatorPanels[1].GetComponentInChildren<MeshRenderer>();

        Material[] changeGlows = new Material[panelMesh.materials.Length];

        for (int i = 0; i < panelMesh.materials.Length; i++)
        {
            if (i == 1)
            {
                changeGlows[i] = elevatorScript.glows[1];
            }
            else
            {
                changeGlows[i] = panelMesh.materials[i];
            }
        }

        panelMesh.materials = changeGlows;
        elevatorAnimator.SetTrigger("Close Door");
        elevatorAnimator.ResetTrigger("Open Door");
        elevatorAnimator.Play("Elevator_InteriorDownButtom");
        GetComponent<Name>().SetText("");
        yield return new WaitForSeconds(2f);
        playerAnimator.Play("Fade Out", 1);
        Debug.Log("End Cutscene");
        yield break;
    }
}
