using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensibilityControl : MonoBehaviour
{
    Slider slider;

    void Awake() => slider = GetComponentInChildren<Slider>();

    void Start() 
    {
        if (PlayerCameraMove.instance != null) slider.value = PlayerCameraMove.instance.sensibility;
        else Debug.LogError("Cant Find PlayerCameraMove");
    }

    void Update() => SetSensibility(slider);

    void SetSensibility(Slider slider) => PlayerCameraMove.instance.sensibility = slider.value; 
}
