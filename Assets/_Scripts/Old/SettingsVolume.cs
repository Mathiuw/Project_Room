using UnityEngine.UI;
using UnityEngine;

public class SettingsVolume : MonoBehaviour
{
    const string playerPrefVolume = "Volume";

    [SerializeField] Slider slider;
    public Slider Slider { get => slider; private set => slider = value; }

    void Start() 
    {       
        Slider.onValueChanged.AddListener(SetVolume);

        Slider.minValue = 0;
        Slider.maxValue = 1;
        Slider.value = PlayerPrefs.GetFloat(playerPrefVolume, 0.25f);

        SetVolume(PlayerPrefs.GetFloat(playerPrefVolume, 0.25f)); 
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(playerPrefVolume, value);
        PlayerPrefs.Save();
    } 
}