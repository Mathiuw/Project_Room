using UnityEngine;

public enum EKeycardColor
{
    Red,
    Green,
    Blue,
    Yellow
}

[CreateAssetMenu(fileName = "New Keycard", menuName = "Keycard")]
public class SOKeycard : SOItem
{
    [Header("Keycard")]
    public EKeycardColor keycardColor;
}
