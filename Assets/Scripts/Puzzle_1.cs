using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_1 : MonoBehaviour
{
    [Header("Garage Door")]
    [SerializeField] Transform gate;
    private Vector3 openPosition;

    [Header("Keycard")]
    [Range(1,4)]
    [SerializeField] private int keycardReaderCount;
    [SerializeField] private GameObject keyCardReader;
    public Items[] keyCards;
    [SerializeField]private bool[] keycardReaderUsed;

    [Header("Glow Materials")]
    [SerializeField] private Material[] glows;

    private Inventory inventory;
    private UI_Inventory uiInventory;

    private void Awake()
    {
        inventory = GameObject.Find("Player_And_Camera").GetComponentInChildren<Inventory>();
        uiInventory = GameObject.Find("Player_And_Camera").GetComponentInChildren<UI_Inventory>();

        openPosition = gate.position;
        gate.localPosition = Vector3.zero;

        Transform keycardReaderPosition = transform.Find("KeyCardReaders").transform;

        float offset = 0;

        if (keycardReaderCount < 1)
        {
            keycardReaderCount = 1;
        }

        for (int i = 0; i < keycardReaderCount; i++)
        {
            GameObject reader = Instantiate(keyCardReader, new Vector3(keycardReaderPosition.position.x, keycardReaderPosition.position.y + offset, keycardReaderPosition.position.z), keycardReaderPosition.rotation, keycardReaderPosition);
            reader.SetActive(true);
            reader.GetComponent<interactive>().necessaryItem = keyCards[i];
            reader.GetComponentInChildren<Name>().text = "Need " + keyCards[i].itemName;
            offset += 1;
        }

        keycardReaderUsed = new bool[keyCards.Length];

        for (int i = 0; i < keycardReaderUsed.Length; i++)
        {
            keycardReaderUsed[i] = false;
        }
    }

    public void UseKeycard(Items keycard,Transform transform)
    {
        for (int i = 0; i < keyCards.Length; i++)
        {
            if (inventory.HasItemOnIndex(keycard) && keycardReaderUsed[i] == false)
            {
                inventory.inventory.RemoveAt(SelectItem.index);
                keycardReaderUsed[i] = true;

                MeshRenderer readerMesh = transform.GetComponentInChildren<MeshRenderer>();
                Material[] changeGlows = new Material[readerMesh.materials.Length];

                for (int g = 0; g < readerMesh.materials.Length; g++)
                {
                    if (g == 1)
                    {
                        changeGlows[g] = glows[2];
                    }
                    else if (g == 2)
                    {
                        changeGlows[g] = glows[0];
                    }
                    else
                    {
                        changeGlows[g] = readerMesh.materials[g];
                    }
                }
                readerMesh.materials = changeGlows;

                transform.GetComponentInChildren<Name>().text = "";
                uiInventory.RefreshInventory();
                CheckPuzzle();
            }
        }
    }

    private void CheckPuzzle()
    {
        for (int i = 0; i < keycardReaderUsed.Length; i++)
        {
            if (keycardReaderUsed[i] == true)
            {
                if (i == keycardReaderUsed.Length - 1)
                {
                    GetComponent<Animator>().Play("GarageDoor_Open");
                    Debug.Log("Gate opened");
                }
            }
            else
            {
                return;
            }
        }
    }

}
