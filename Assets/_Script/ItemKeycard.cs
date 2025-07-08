using UnityEngine;

public class ItemKeycard : Item
{
    [SerializeField] private Material[] materials = new Material[4];

    // Make the keycard material color according to the enum type
    private void Awake()
    {
        SOKeycard soKeycard = (SOKeycard)SOItem;
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();

        switch (soKeycard.keycardColor)
        {
            case EKeycardColor.Red:
                mesh.materials[0] = materials[0];
                break;
            case EKeycardColor.Green:
                mesh.materials[0] = materials[1];
                break;
            case EKeycardColor.Blue:
                mesh.materials[0] = materials[2];
                break;
            case EKeycardColor.Yellow:
                mesh.materials[0] = materials[3];
                break;
            default:
                Debug.LogError("Error setting keycard color");
                break;
        }
    }

}
