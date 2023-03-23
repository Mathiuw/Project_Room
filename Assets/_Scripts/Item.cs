using UnityEngine;

public class Item : MonoBehaviour
{
    public SOItem item;
    public int amount = 1;

    void Start()
    {
        SpawnItem();
    }

    void SpawnItem()
    {
        Transform itemTransform = item.itemPrefab.GetComponent<Transform>();

        amount = 1;
        gameObject.name = item.itemName;
        transform.eulerAngles = new Vector3(itemTransform.eulerAngles.x, transform.eulerAngles.y, itemTransform.eulerAngles.z);
        GetComponent<Name>().SetText(item.itemName);
        GameObject prefab = Instantiate(item.itemPrefab,transform);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.rotation = item.itemPrefab.transform.rotation;
    }
}
