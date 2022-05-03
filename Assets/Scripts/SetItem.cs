using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : MonoBehaviour
{
    public int amount = 1;

    public Items item;

    private void Start()
    {
        SpawnItem();
    }

    private void SpawnItem()
    {
        Transform itemTransform = item.itemPrefab.GetComponent<Transform>();

        amount = 1;
        gameObject.name = item.itemName;
        transform.eulerAngles = new Vector3(itemTransform.eulerAngles.x, transform.eulerAngles.y, itemTransform.eulerAngles.z);
        GetComponent<Name>().text = item.itemName;
        GameObject prefab = Instantiate(item.itemPrefab,gameObject.transform);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.rotation = item.itemPrefab.transform.rotation;
    }
}
