using System;
using UnityEngine;

public class KeycardReader : MonoBehaviour, IInteractable, IUIName
{
    public SOKeycard keycardNeeded { get; set; }

    public bool used { get; set; } = false;

    public Material offMaterial { get; set; }
    public Material acceptedMaterial { get; set; }
    public Material recusedMaterial { get; set; }

    public string ReadName => SetReadName();

    public event Action onAccept;

    public void Interact(Transform interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();

        if (inventory.HaveItemSelected(keycardNeeded))
        {
            inventory.RemoveItem(keycardNeeded);
            used = true;
            ChangeMeshMaterials();

            onAccept?.Invoke();
        }
    }

    void ChangeMeshMaterials() 
    {
        Material[] materials = transform.GetComponentInChildren<MeshRenderer>().materials;

        materials[1] = offMaterial;
        materials[2] = acceptedMaterial;

        GetComponentInChildren<MeshRenderer>().materials = materials;
    }

    private string SetReadName() 
    {
        if (!used)
        {
            return "Need " + keycardNeeded.itemName;
        }
        else return "";
    }
}
