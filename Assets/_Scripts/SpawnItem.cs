using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public SOItem itemSO;

    void Start()
    {
        Spawn();
    }

    public void Spawn() 
    {
        if (itemSO == null)
        {
            Debug.LogError("Item is null");
            return;
        }

        //Add an ser item component
        Item itemComponent = gameObject.AddComponent<Item>();
        itemComponent.SOItem = itemSO;
        itemComponent.amount = 1;

        name = itemSO.itemName;
        GetComponent<ShowNameToHUD>().SetText(itemSO.itemName);

        //Spawn item mesh 
        Instantiate(itemSO.itemPrefab, transform);

        Destroy(this);
    }
}
