using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Database/WeaponDatabase")]
public class SOWeaponDatabase : ScriptableObject
{
    public List<SOWeapon> allWeapons;
}
