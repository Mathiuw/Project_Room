using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGarage : MonoBehaviour
{
    [Header("Garage Door")]
    [SerializeField] Transform gate;
    Vector3 openPosition;

    [Header("Keycard Reader")]
    [Range(1,4)]
    [SerializeField] int count;
    [SerializeField] KeycardReader ReaderPrefab;
    KeycardReader[] Readers;

    [SerializeField] public SOItem[] keyCards;

    [Header("Glow Materials")]
    [SerializeField] Material[] glows;

    void Start()
    {
        if (count < 1) count = 1;

        Readers = new KeycardReader[count];

        SetDoor();
        SetKeycardReaders();    
    }

    void SetKeycardReaders() 
    {
        float offset = 0;
        Transform ReadersTransform = transform.Find("KeycardReaders");

        for (int i = 0; i < count; i++)
        {
            KeycardReader reader = Instantiate(ReaderPrefab,
                new Vector3(ReadersTransform.position.x,
                ReadersTransform.position.y + offset,
                ReadersTransform.position.z),
                ReadersTransform.rotation,
                ReadersTransform);

            reader.acceptedMaterial = glows[0];
            reader.recusedMaterial = glows[1];
            reader.offMaterial = glows[2];

            reader.onAccept += CanOpenDoor;

            Readers[i] = reader;

            reader.GetComponent<KeycardReader>().keycard = keyCards[i];
            reader.GetComponentInChildren<Name>().SetText("Need " + keyCards[i].itemName);
            offset += 1;
        }
    }

    void SetDoor() 
    {
        openPosition = gate.position;
        gate.position = Vector3.zero;
    }

    void CanOpenDoor()
    {
        Debug.Log("check if can open doors");

        for (int i = 0; i < Readers.Length; i++)
        {
            if (Readers[i].used) continue;
            else return;
        }

        GetComponent<Animator>().Play("GarageDoor_Open");
        OpenDoor();
        Debug.Log("Gate opened");
    }

    void OpenDoor() 
    {
        gate.position = Vector3.Lerp(gate.position, openPosition, Time.deltaTime);
    }
}
