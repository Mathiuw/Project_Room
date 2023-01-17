using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponAnimations : MonoBehaviour
{    
    [SerializeField] AnimatorOverrideController weaponOverrideController;
    public AnimatorOverrideController WeaponOverrideController { get => weaponOverrideController; set => weaponOverrideController = value; }
}
