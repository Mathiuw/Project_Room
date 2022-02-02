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

    }

    public void OpenElevator()
    {
        Animator elevatorAnimator = GetComponentInParent<Animator>();

        elevatorAnimator.SetTrigger("Open Door");
        elevatorAnimator.ResetTrigger("Close Door");
        transform.GetComponent<Name>().text = "";
        interactiveTypes = InteractiveTypes.None;
    }

    IEnumerator CloseElevatorAndSwitchLevel()
    {
        Animator elevatorAnimator = GetComponentInParent<Animator>();

        interactiveTypes = InteractiveTypes.None;
        elevatorAnimator.SetTrigger("Close Door");
        elevatorAnimator.ResetTrigger("Open Door");
        GetComponent<Name>().text = "";
        yield return new WaitForSeconds(3);
        FindObjectOfType<gameManagent>().StartCoroutine("NextLevel");
        yield return new WaitForSeconds(3);
        elevatorAnimator.SetTrigger("Open Door");
        elevatorAnimator.ResetTrigger("Close Door");
        yield break;
    }
}
