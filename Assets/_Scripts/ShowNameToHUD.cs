using UnityEngine;

public class ShowNameToHUD : MonoBehaviour
{
    [SerializeField] string text;

    public string GetText() { return text; }
    public void SetText(string text) { this.text = text; } 
}
