using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsResolution : MonoBehaviour
{
    Resolution[] resolutions;

    [SerializeField] TMP_Dropdown dropdown;

    void Start()
    {
        resolutions = Screen.resolutions;

        dropdown.ClearOptions();

        List<string> resolutionsOptions = new List<string>();

        int curentResolutionindex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolution = resolutions[i].width + " x " + resolutions[i].height;

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                curentResolutionindex = i;
            }

            resolutionsOptions.Add(resolution);
        }

        dropdown.AddOptions(resolutionsOptions);

        dropdown.value = curentResolutionindex;
        dropdown.RefreshShownValue();
        SetResolution(curentResolutionindex);
    }

    public void SetResolution(int index) 
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
