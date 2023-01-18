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
        weaponInteraction = Player.Instance.GetComponentInChildren<WeaponInteraction>();

        weaponInteraction.onPickupEnd += OnPickup;
        weaponInteraction.onWeaponDrop -= OnPickup;
    }

    void OnPickup(Transform gun)
    {
        shakeValues = gun.GetComponentInParent<ReturnShakeValues>();

        amplitude = shakeValues.amplitude;
        frequency = shakeValues.frequency;
        duration = shakeValues.duration;

        gun.GetComponentInParent<ShootGun>().onShoot += StartShake;
    }

    void OnDrop(Transform gun)
    {
        amplitude = 0;
        frequency = 0;
        duration = 0;

        gun.GetComponentInParent<ShootGun>().onShoot -= StartShake;
    }
}
