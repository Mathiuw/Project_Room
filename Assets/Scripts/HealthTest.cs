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
    [SerializeField] private int amount;

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();

        if (!collision.gameObject.GetComponent<Health>()) return;

        switch (type)
        {
            case Type.Remove:
                health.RemoveHealth(amount);
                break;
            case Type.add:
                health.AddHealth(amount);
                break;
        }
    }
}
