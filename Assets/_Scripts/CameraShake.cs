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

    protected void SetShake() 
    {
        cameraNoise.m_AmplitudeGain = amplitude;
        cameraNoise.m_FrequencyGain = duration;
    } 
}
