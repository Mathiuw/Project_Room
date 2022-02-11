using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactive : MonoBehaviour
{
    public InteractiveTypes interactiveTypes;

    public Items necessaryItem;

    private Puzzle_1 puzzleScript;

    public enum InteractiveTypes
    {
        None,
        keycard,
        Door,
        OpenElevator,
        CloseElevatorAndSwitchLevel,
    }

    private void Awake()
    {
        if (interactiveTypes == InteractiveTypes.keycard)
        {
            puzzleScript = GetComponentInParent<Puzzle_1>();
        }
        else if (interactiveTypes == InteractiveTypes.Door)
        {
            Animator DoorAnimator = GetComponentInParent<Animator>();

            DoorAnimator.SetBool("Open Door", false);
            DoorAnimator.SetBool("Close Door", true);
            foreach (Name name in GetComponentsInChildren<Name>())
            {
                name.text = "Open Door";
            }  
        }
    }

    public void Interact()
    {
        switch (interactiveTypes)
        {
            case InteractiveTypes.None:
                return;

            case InteractiveTypes.keycard:
                puzzleScript.UseKeycard(necessaryItem);
                break;

            case InteractiveTypes.Door:
                UseDoor();
                break;

            case InteractiveTypes.OpenElevator:
                OpenElevator();
                break;

            case InteractiveTypes.CloseElevatorAndSwitchLevel:
                StartCoroutine(CloseElevatorAndSwitchLevel());
                break;
        }
    }

    public void UseDoor()
    {
        Animator DoorAnimator = GetComponentInParent<Animator>();

        if (DoorAnimator.GetBool("Open Door") == true)
        {
            DoorAnimator.SetBool("Open Door",false);
            DoorAnimator.SetBool("Close Door",true);
            foreach (Name name in GetComponentsInChildren<Name>())
            {
                name.text = "Open Door";
            }
        }
        else if (DoorAnimator.GetBool("Close Door") == true)
        {
            DoorAnimator.SetBool("Close Door", false);
            DoorAnimator.SetBool("Open Door", true);
            foreach (Name name in GetComponentsInChildren<Name>())
            {
                name.text = "Close Door";
            }
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
        transform.GetComponent<Name>().text = "";
        interactiveTypes = InteractiveTypes.None;
    }

    IEnumerator CloseElevatorAndSwitchLevel()
    {
        Animator elevatorAnimator = GetComponentInParent<Animator>();
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
        GetComponent<Name>().text = "";
        interactiveTypes = InteractiveTypes.None;
        yield return new WaitForSeconds(3);
        FindObjectOfType<gameManagent>().StartCoroutine("NextLevel");
        yield return new WaitForSeconds(3);
        elevatorAnimator.SetTrigger("Open Door");
        elevatorAnimator.ResetTrigger("Close Door");
        yield break;
    }
}
