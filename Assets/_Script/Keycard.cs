using UnityEngine;

public class Keycard : Item
{
    [SerializeField] private Material[] materials = new Material[4];

    private void Awake()
    {
        SOKeycard soKeycard = (SOKeycard)SOItem;
        Material[] materials = GetComponentInChildren<MeshRenderer>().materials;
        switch (soKeycard.keycardColor)
        {
            case EKeycardColor.Red:
                materials[0] = this.materials[0];
                break;
            case EKeycardColor.Green:
                materials[0] = this.materials[1]; 
                break;
            case EKeycardColor.Blue:
                materials[0] = this.materials[2];
                break;
            case EKeycardColor.Yellow:
                materials[0] = this.materials[3];
                break;
            default:
                Debug.LogError("Error setting keycard color");
                break;
        }

        GetComponentInChildren<MeshRenderer>().materials = materials;
    }

}
