using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : MonoBehaviour
{
    public int amount = 1;

    public Items item;

    private void Start()
    {
        amount = 1;
        gameObject.name = item.itemName;
        transform.rotation = item.itemPrefab.GetComponent<Transform>().localRotation;
        transform.localScale = item.itemPrefab.GetComponent<Transform>().localScale;
        GetComponent<Name>().text = item.itemName;
        GetComponent<MeshFilter>().sharedMesh = item.itemPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        GetComponent<MeshCollider>().cookingOptions = item.itemPrefab.GetComponentInChildren<MeshCollider>().cookingOptions;
        GetComponent<MeshCollider>().sharedMesh = item.itemPrefab.GetComponentInChildren<MeshCollider>().sharedMesh;
        GetComponent<MeshRenderer>().sharedMaterials = item.itemPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials;
    }
}
