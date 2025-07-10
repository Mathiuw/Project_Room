using System;
using UnityEngine;

public class KeycardReader : MonoBehaviour, IInteractable, IUIName
{
    public SOKeycard keycardNeeded { get; set; }

    public bool Used { get; set; } = false;

    public Material offMaterial { get; set; }
    public Material acceptedMaterial { get; set; }
    public Material recusedMaterial { get; set; }

    public string ReadName => SetReadName();

    public event Action OnAcceptKeycard;

    public void Interact(Transform interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();

        if (inventory.HaveItem(keycardNeeded))
        {
            inventory.RemoveItem(keycardNeeded);
            Used = true;
            ChangeMeshMaterials();

            OnAcceptKeycard?.Invoke();
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
        if (!Used)
        {
            return "Need " + keycardNeeded.itemName;
        }
        else return "";
    }
}
