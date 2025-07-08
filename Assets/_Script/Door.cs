using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IUIName
{
    [Header("Name")]
    [SerializeField] string doorName = "Door";
    
    [Header("Rotation")]
    [SerializeField] float duration = 0.4f;
    [SerializeField] Transform[] doors;
    [SerializeField] Vector3[] startRotation;
    [SerializeField] Vector3[] desiredRotations;

    [Header("Destruction")]
    [SerializeField] bool isDestrucble = true;

    public bool open { get; private set; } = false;

    public string ReadName => GetUIName();

    private void Awake()
    {
        for (int i = 0; i < doors.Length; i++) doors[i].localEulerAngles = startRotation[i];
    }

    public void Interact(Transform interactor)
    {
        StartCoroutine(OpenCloseDoor());
    }

    IEnumerator OpenCloseDoor() 
    {
        enabled= false;

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

        // Invert open state
        open = !open;

        enabled = true;
        yield break;
    }

    // Animacao procedural para porta
    void ArrayLerp(Transform[] t, Vector3[] startRotation, Vector3[] desiredRotation, float percentageComplete ) 
    {
        for (int i = 0; i < doors.Length; i++)
        {
            t[i].localRotation = Quaternion.Lerp( Quaternion.Euler(startRotation[i]), Quaternion.Euler(desiredRotation[i]), percentageComplete);
        }
    }

    // Logica para destruir a porta
    public void DestroyDoor(Vector3 direction, float force)
    {
        if (!isDestrucble)
        {
            Debug.Log("Door is not destrucble");
            return;
        }

        Door doorScript;

        if (doorScript = GetComponent<Door>())
        {
            if (doorScript.open) return;
            Destroy(doorScript);
        }

        foreach (Transform door in doors)
        {
            door.SetParent(null);

            Rigidbody doorRB = door.GetComponentInChildren<Rigidbody>();

            doorRB.isKinematic = false;
            doorRB.interpolation = RigidbodyInterpolation.Interpolate;
            doorRB.AddForce(direction * force, ForceMode.VelocityChange);
            doorRB.AddTorque(direction * force, ForceMode.VelocityChange);
        }
    }

    string GetUIName() 
    {
        if (open) return "Close " + doorName;
        else return "Open " + doorName;
    }
}
