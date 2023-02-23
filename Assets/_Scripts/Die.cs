using UnityEngine;
using System;

public class Die : MonoBehaviour
{
    public event Action onDead;
    bool isDead = false;

    public void Dead() 
    {
        if (isDead) return;

        isDead = true;
        onDead?.Invoke();;
        onDead = null;
    } 
}