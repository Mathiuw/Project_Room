using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAndDropItems : MonoBehaviour
{
    [Header("Pickup item")]
    [SerializeField] private float rayLenght;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask itemMask;

    [Header("Drop item")]
    [SerializeField] private GameObject itemPrefab;

    public void pickupItem()
    {
        RaycastHit hit;      

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLenght, itemMask))
        {
            if (hit.transform.GetComponent<SetItem>() && Player.Instance.Inventory.CheckAndAddItem(hit.transform.GetComponent<SetItem>()))
            {
                Player.Instance.UIInventory.RefreshInventory();
                Destroy(hit.transform.gameObject);
                Debug.Log("Picked item");
            }
        }
    }

    public void UseItem()
    {
        foreach (SetItem item in Player.Instance.Inventory.inventory)
        {
            if (Player.Instance.Inventory.inventory.IndexOf(item) == SelectItem.index && item.item.isConsumable)
            {
                GetComponent<Health>().AddHealth(item.item.recoverHealth);

                if (item.item.canInfiniteSprint)
                {               
                    Player.Instance.Sprint.infiniteSprint += OnAdrenalineUsed;
                    Player.Instance.Sprint.InfiniteSprintEvent(item.item.infiniteAdrenalineDuration);
                }

                if (item.amount > 1) item.amount--;
                else Player.Instance.Inventory.inventory.Remove(item);

                Player.Instance.UIInventory.RefreshInventory();
                Debug.Log(item.item.name + " used and removed");
                break;
            }
        }
    }

    IEnumerator OnAdrenalineUsed(float time)
    {
        Sprint sprintScript = GetComponent<Sprint>();

        sprintScript.isInfinite = true;
        Debug.Log("Infinite Sprint Started");

        yield return new WaitForSeconds(time);

        sprintScript.isInfinite = false;
        Debug.Log("Infinite Sprint Finished");
        yield break;
    }

    public void DropItem()
    {
        SpawnDropItem();
        Player.Instance.UIInventory.RefreshInventory();
        Debug.Log("Item Droped");
    }

    void SpawnDropItem()
    {
        foreach (SetItem item in Player.Instance.Inventory.inventory)
        {
            if (Player.Instance.Inventory.inventory.IndexOf(item) == SelectItem.index)
            {
                if (item.amount == 1) Player.Instance.Inventory.inventory.RemoveAt(SelectItem.index);
                else item.amount--;
                GameObject itemSpawned = Instantiate(itemPrefab, cameraTransform.position + cameraTransform.forward * 1.5f, cameraTransform.rotation);
                itemSpawned.GetComponent<SetItem>().item = item.item;
                itemSpawned.GetComponent<Rigidbody>().AddForce(cameraTransform.forward * 5, ForceMode.VelocityChange);
                break;
            }
        }
    }
}