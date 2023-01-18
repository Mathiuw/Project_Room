using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;

public class CameraShake : MonoBehaviour
{
    protected CinemachineBasicMultiChannelPerlin cameraNoise;
    protected float amplitude = 0;
    protected float frequency = 0;
    protected float duration = 0;

    protected void StartShake() => StartCoroutine(ShakeCamera());

    protected IEnumerator ShakeCamera() 
    {
        cameraNoise.m_AmplitudeGain = amplitude;
        cameraNoise.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        float elapsedTime = 0;
        float percentageComplete = 0;

        while (elapsedTime > duration)
        {
            cameraNoise.m_AmplitudeGain = Mathf.Lerp(amplitude, 0, percentageComplete);
            cameraNoise.m_FrequencyGain = Mathf.Lerp(frequency, 0, percentageComplete);

            elapsedTime += Time.deltaTime;
            percentageComplete += elapsedTime / duration;
        }

        cameraNoise.m_AmplitudeGain = 0;
        cameraNoise.m_FrequencyGain = 0;

        yield break;
    }
}
