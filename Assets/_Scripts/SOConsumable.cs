using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Consumable")]
public class SOConsumable : SOItem
{
    [Header("Consumable effects")]
    public int recoverHealth;
}
