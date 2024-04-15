using UnityEngine;
using UnityEngine.UI;

public class SettingsSensibility : MonoBehaviour
{
    const string playerPrefSensibility = "sensibility";

    [SerializeField] Slider slider;
    public Slider Slider { get => slider; private set => slider = value; }

    PlayerCameraMove playerCameraMoveScript;

    void Start() 
    {
        Slider.onValueChanged.AddListener(SetSensibility);

        Slider.minValue = 1; 
        Slider.maxValue = 100;
        Slider.value = PlayerPrefs.GetFloat(playerPrefSensibility, 40);

        SetSensibility(PlayerPrefs.GetFloat(playerPrefSensibility, 40)); 

        playerCameraMoveScript = FindAnyObjectByType<PlayerCameraMove>();
    }

    public void SetSensibility(float value) 
    {
        PlayerPrefs.SetFloat(playerPrefSensibility, value);
        
        if (playerCameraMoveScript) playerCameraMoveScript.SetSensiblility(value);

        PlayerPrefs.Save();
    } 
}
