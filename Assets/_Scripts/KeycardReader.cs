using System;
using UnityEngine;

[RequireComponent(typeof(ShowNameToHUD))]
public class KeycardReader : MonoBehaviour, IInteractable
{
    public SOItem keycard { get; set; }

    public bool used { get; set; } = false;

    public Material offMaterial { get; set; }
    public Material acceptedMaterial { get; set; }
    public Material recusedMaterial { get; set; }

    public event Action onAccept;

    public void Interact(Transform interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();

        if (inventory.HaveItemSelected(keycard))
        {
            inventory.RemoveItem(keycard);
            transform.GetComponentInChildren<ShowNameToHUD>().SetText("");
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
}
