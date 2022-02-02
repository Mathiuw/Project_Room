using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : MonoBehaviour
{
    private enum Type
    {
        Remove,
        add,
    }

    [SerializeField] private Type type;
    [SerializeField] private LayerMask playermask;
    [SerializeField] private int amount;

    private void OnCollisionEnter(Collision collision)
    {
        switch (type)
        {
            case Type.Remove:
                Health.RemoveHealth(amount);
                Debug.Log("Player Health = " + Health.playerHealth);
                break;
            case Type.add:
                Health.AddHealth(amount);
                Debug.Log("Player Health = " + Health.playerHealth);
                break;
        }
    }
}
