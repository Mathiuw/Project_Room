using UnityEngine;

[CreateAssetMenu(fileName = "New Keycard", menuName = "Keycard")]
public class SOKeycard : SOItem
{
    [Header("Keycard")]
    public KeycardColor keycardColor;

    public enum KeycardColor 
    {
        Red,
        Green, 
        Blue,
        Yellow
    }
}
