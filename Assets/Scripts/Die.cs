using UnityEngine;
using System;

public class Die : MonoBehaviour
{
    public event Action Died;
    bool isDead = false;

    //Morre(Obvio)
    public void Dead() 
    {
        if (isDead) return;

        isDead = true;
        Died?.Invoke();;
    } 
}
