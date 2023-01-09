using UnityEngine;
using System;

[RequireComponent(typeof(Health))]
public class Die : MonoBehaviour
{
    public event Action Died;

    //Morre(Obvio)
    public void Dead() => Died?.Invoke();
}
