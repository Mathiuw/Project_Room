using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = transform.Find("VolumeSlider").GetComponent<Slider>();
    }

    private void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }

    private void Update()
    {
        AudioListener.volume = volumeSlider.value;
    }
}