using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public SOItem item;

    void Start()
    {
        Spawn();
    }

    public void Spawn() 
    {
        if (item == null)
        {
            Debug.LogError("Item is null");
            return;
        }

        //Add an ser item component
        Item itemComponent = gameObject.AddComponent<Item>();
        itemComponent.SOItem = item;
        itemComponent.amount = 1;

        gameObject.name = item.itemName;
        GetComponent<Name>().SetText(item.itemName);

        //Spawn item mesh 
        Instantiate(item.itemPrefab, transform);

        Destroy(this);
    }
}
