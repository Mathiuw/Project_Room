using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShowSliderValue : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float multiplier;
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        slider.onValueChanged.AddListener(ShowValue);
        ShowValue(slider.value);
    }

    void ShowValue(float value) 
    {
        float valueMultiplied = value * multiplier;

        text.text = valueMultiplied.ToString("0");
    }
}
