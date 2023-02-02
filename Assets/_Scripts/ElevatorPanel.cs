using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Name))]
public class ElevatorPanel : Interact
{
    bool used = false;

    [SerializeField] Buttom buttom;

    enum Buttom { up, down }

    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;

    MeshRenderer mesh;
    Name panelName;

    public event Action onButtomPress;

    void Awake() 
    {
        panelName = GetComponent<Name>();
        mesh = GetComponentInChildren<MeshRenderer>();
    } 

    void Start() 
    {
        SetMaterials(mesh);
        SetText(panelName);

        if (buttom == Buttom.down) onButtomPress += ManagerGame.instance.EndGame;
    } 

    public override void Interacting(Transform t)
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

    void SetText(Name name) 
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
