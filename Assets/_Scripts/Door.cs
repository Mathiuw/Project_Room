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
    [SerializeField] Vector3[] startRotation;
    [SerializeField] Vector3[] desiredRotations;
    bool open = false;
    Name doorName;

    void Start() 
    {
        doorName = GetComponent<Name>();

        SetName(objectName);

        for (int i = 0; i < doors.Length; i++) doors[i].localEulerAngles = startRotation[i];
    }

    public override void Interacting(Transform t) => StartCoroutine(OpenCloseDoor());

    IEnumerator OpenCloseDoor() 
    {
        enabled= false;
        doorName.enabled = false;

        float elapsedtime = 0f;     
        float percentageComplete = 0f;

        while (elapsedtime < duration)
        {
            if (!open) ArrayLerp(doors, startRotation, desiredRotations, percentageComplete);
            else ArrayLerp(doors, desiredRotations, startRotation, percentageComplete);

            elapsedtime += Time.deltaTime;
            percentageComplete = elapsedtime / duration;

            yield return null;
        }
        for (int i = 0; i < doors.Length; i++) 
        {
            if (!open) doors[i].localRotation = Quaternion.Euler(desiredRotations[i]);
            else doors[i].localRotation = Quaternion.Euler(startRotation[i]);
        }

        open = !open;
        SetName(objectName);

        doorName.enabled= true;
        enabled = true;
        yield break;
    }

    void ArrayLerp(Transform[] t, Vector3[] startRotation, Vector3[] desiredRotation, float percentageComplete ) 
    {
        for (int i = 0; i < doors.Length; i++)
        {
            t[i].localRotation = Quaternion.Lerp( Quaternion.Euler(startRotation[i]), Quaternion.Euler(desiredRotation[i]), percentageComplete);
        }
    }

    void ChangeNames(string text)=> doorName.SetText(text);

    void SetName(string text) 
    {
        if (open) ChangeNames("Close " + text);
        else ChangeNames("Open " + text);
    }
}
