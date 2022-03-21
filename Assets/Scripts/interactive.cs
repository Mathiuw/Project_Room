using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactive : MonoBehaviour
{
    public InteractiveTypes interactiveTypes;

    [Header("KeyCard Reader")]
    public Items necessaryItem;

    [Header("Doors")]
    [SerializeField] private string objectDoor;

    private Puzzle_1 puzzleScript;

    private Transform player;

    public enum InteractiveTypes
    {
        None,
        keycard,
        Door,
        OpenElevator,
        CloseElevatorAndEndGame,
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
                name.text = "Open " + objectDoor;
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
                puzzleScript.UseKeycard(necessaryItem, transform);
                break;

            case InteractiveTypes.Door:
                UseDoor();
                break;

            case InteractiveTypes.OpenElevator:
                OpenElevator();
                break;

            case InteractiveTypes.CloseElevatorAndEndGame:
                StartCoroutine(CloseElevatorAndEndGame());
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
                name.text = "Open " + objectDoor;
            }
        }
        else if (DoorAnimator.GetBool("Close Door") == true)
        {
            DoorAnimator.SetBool("Close Door", false);
            DoorAnimator.SetBool("Open Door", true);
            foreach (Name name in GetComponentsInChildren<Name>())
            {
                name.text = "Close " + objectDoor;
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

    IEnumerator CloseElevatorAndEndGame()
    {
        Animator elevatorAnimator = GetComponentInParent<Animator>();
        Animator playerAnimator = player.GetComponent<Animator>();
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
        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetFloat("FadeSpeed",-1);
        yield break;
    }
}
