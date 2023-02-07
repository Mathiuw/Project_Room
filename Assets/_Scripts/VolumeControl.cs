using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    Slider volumeSlider;

    void Awake() => volumeSlider = GetComponentInChildren<Slider>();

    void Start() => volumeSlider.value = AudioListener.volume;

    void Update() => SetVolume(volumeSlider);

    void SetVolume(Slider slider) => AudioListener.volume = slider.value;
}