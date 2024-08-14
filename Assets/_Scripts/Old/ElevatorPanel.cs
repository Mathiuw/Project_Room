using System;
using UnityEngine;

[RequireComponent(typeof(ShowNameToHUD))]
public class ElevatorPanel : MonoBehaviour, IInteractable
{
    bool used = false;

    [SerializeField] Buttom buttom;

    enum Buttom { up, down }

    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;

    MeshRenderer mesh;
    ShowNameToHUD panelName;

    public event Action onButtomPress;

    void Awake() 
    {
        panelName = GetComponent<ShowNameToHUD>();
        mesh = GetComponentInChildren<MeshRenderer>();
    } 

    void Start() 
    {
        SetMaterials(mesh);
        SetText(panelName);

        if (buttom == Buttom.down) onButtomPress += ManagerGame.instance.EndGame;
    }

    public void Interact(Transform interactor)
    {
        used = !used;
        SetMaterials(mesh);
        SetText(panelName);
        onButtomPress?.Invoke();
        Destroy(this);
    }

    void SetMaterials(MeshRenderer mesh) 
    {
        Material[] materials= mesh.materials;

        if (buttom == Buttom.up && !used)
        {
            materials[1] = offMaterial;
            materials[2] = onMaterial;
        }
        else if (used)
        {
            materials[1] = offMaterial;
            materials[2] = offMaterial;
        }

        mesh.materials = materials;
    }

    void SetText(ShowNameToHUD name) 
    {
        if (used)
        {
            panelName.SetText("");
            return;
        }

        if (buttom == Buttom.up) name.SetText("Call Elevator");
        else name.SetText("Close Elevator");
    }

}
