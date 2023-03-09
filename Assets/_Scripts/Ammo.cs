using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int ammo { get; private set; } 
    public int maxAmmo { get; private set; } 

    public void SetAttributes(int maxAmmo) => this.maxAmmo = maxAmmo; 

    IEnumerator Start() 
    {
        yield return new WaitForEndOfFrame();
        AddAmmo(maxAmmo); 
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maxAmmo) ammo = maxAmmo;
    }

    public void RemoveAmmo(int amount) => ammo -= amount;
}
