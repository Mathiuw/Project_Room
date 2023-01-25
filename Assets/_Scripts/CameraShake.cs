using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;

public class CameraShake : MonoBehaviour
{
    protected CinemachineBasicMultiChannelPerlin cameraNoise;
    protected float amplitude = 0;
    protected float duration = 0;

    protected void StartShake() => StartCoroutine(ShakeCamera());

    protected IEnumerator ShakeCamera() 
    {
        cameraNoise.m_AmplitudeGain = amplitude;
        cameraNoise.m_FrequencyGain = duration;

        while (cameraNoise.m_AmplitudeGain > 0f)
        {
            cameraNoise.m_AmplitudeGain -= Time.deltaTime;
        }
        cameraNoise.m_AmplitudeGain = 0;

        yield break;
    }
}
