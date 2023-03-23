using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Name))]
public class KeycardReader : Interact
{
    public SOItem keycard { get; set; }

    public bool used { get; set; } = false;

    public Material offMaterial { get; set; }
    public Material acceptedMaterial { get; set; }
    public Material recusedMaterial { get; set; }

    public event Action onAccept;

    public override void Interacting(Transform t)
    {
        Inventory inventory= t.GetComponent<Inventory>();

        if (inventory.HaveItemSelected(keycard))
        {
            inventory.RemoveItem(keycard);
            transform.GetComponentInChildren<Name>().SetText("");
            UI_Inventory.instance.RefreshInventory();
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
