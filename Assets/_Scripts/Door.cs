using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Name))]
public class Door : Interact
{
    [Header("Object Name")]
    [SerializeField] string objectName = "Door";
    
    [Header("Rotation")]
    [SerializeField] float duration = 0.4f;
    [SerializeField] Transform[] doors;
    [SerializeField] Vector3[] desiredRotations;
    bool open = false;

    Name doorName;

    void Awake() 
    {
        doorName = GetComponent<Name>();

        SetName(objectName);
    }

    public override void Interacting() => StartCoroutine(OpenCloseDoor());

    IEnumerator OpenCloseDoor() 
    {
        enabled= false;
        doorName.enabled = false;

        float elapsedtime = 0f;     
        float percentageComplete = 0f;

        Quaternion[] startRotation = new Quaternion[doors.Length];

        for (int i = 0; i < doors.Length; i++) startRotation[i] = doors[i].localRotation;

        while (elapsedtime < duration)
        {
            if (!open)
                for (int i = 0; i < doors.Length; i++)
                {
                    doors[i].localRotation = Quaternion.Lerp(startRotation[i], Quaternion.Euler(desiredRotations[i]), percentageComplete);
                }
            else
                for (int i = 0; i < doors.Length; i++) 
                {
                    doors[i].localRotation = Quaternion.Lerp(startRotation[i], Quaternion.identity, percentageComplete);
                }       

            elapsedtime += Time.deltaTime;
            percentageComplete = elapsedtime / duration;

            yield return null;
        }
        for (int i = 0; i < doors.Length; i++) 
        {
            if (!open) doors[i].localRotation = Quaternion.Euler(desiredRotations[i]);
            else doors[i].localRotation = Quaternion.identity;
        }

        open = !open;
        SetName(objectName);

        doorName.enabled= true;
        enabled = true;
        yield break;
    }

    void ChangeNames(string text)=> doorName.text = text;

    void SetName(string text) 
    {
        if (open) ChangeNames("Close " + text);
        else ChangeNames("Open " + text);
    }
}
