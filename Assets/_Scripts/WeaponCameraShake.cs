using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCameraShake : CameraShake
{
    ReturnShakeValues shakeValues;
    WeaponInteraction weaponInteraction;

    void Start() 
    {
        cameraNoise = GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        weaponInteraction = Player.instance.GetComponentInChildren<WeaponInteraction>();

        weaponInteraction.onPickupEnd += OnPickup;
    }

    void Update() 
    {
        while (cameraNoise.m_AmplitudeGain > 0f)
        {
            cameraNoise.m_AmplitudeGain -= Time.deltaTime / duration;
            return;
        }
        cameraNoise.m_AmplitudeGain = 0;
    }

    void OnPickup(Transform gun)
    {
        shakeValues = gun.GetComponentInParent<ReturnShakeValues>();

        amplitude = shakeValues.amplitude;
        duration = shakeValues.duration;

        gun.GetComponentInParent<ShootGun>().onShoot += SetShake;
    }

    void OnDrop(Transform gun)
    {
        amplitude = 0;
        duration = 0;
        duration = 0;

        gun.GetComponentInParent<ShootGun>().onShoot -= SetShake;
    }
}
