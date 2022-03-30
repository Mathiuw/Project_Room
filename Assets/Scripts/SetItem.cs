using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
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
        transform.localScale = itemTransform.localScale;
        GetComponent<Name>().text = item.itemName;
        GetComponent<MeshFilter>().sharedMesh = item.itemPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        GetComponent<MeshCollider>().cookingOptions = item.itemPrefab.GetComponentInChildren<MeshCollider>().cookingOptions;
        GetComponent<MeshCollider>().sharedMesh = item.itemPrefab.GetComponentInChildren<MeshCollider>().sharedMesh;
        GetComponent<MeshRenderer>().sharedMaterials = item.itemPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials;
    }
}
