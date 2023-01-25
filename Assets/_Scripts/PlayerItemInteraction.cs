using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    [Header("Pickup item")]
    Transform cameraTransform;
    Inventory inventory;
    Sprint sprint;

    [Header("Drop item")]
    [SerializeField] GameObject itemPrefab;

    void Awake() 
    {
        inventory = GetComponent<Inventory>();
        cameraTransform = Camera.main.transform;
        sprint= GetComponent<Sprint>();
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Q)) DropItem();

        if (Input.GetKeyDown(KeyCode.F)) UseItem();
    }

    public void UseItem()
    {
        foreach (SetItem item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == UI_SelectItem.index && item.item.isConsumable)
            {
                GetComponent<Health>().AddHealth(item.item.recoverHealth);

                //if (item.item.canInfiniteSprint)
                //{               
                //    Player.Instance.Sprint.InfiniteSprinted += OnAdrenalineUsed;
                //    Player.Instance.Sprint.OnInfiniteSprint(item.item.infiniteAdrenalineDuration);
                //}

                if (item.amount > 1) item.amount--;
                else inventory.inventory.Remove(item);

                UI_Inventory.instance.RefreshInventory();
                Debug.Log(item.item.name + " used and removed");
                break;
            }
        }
    }

    IEnumerator OnAdrenalineUsed(float time)
    {
        Sprint sprintScript = GetComponent<Sprint>();

        sprint.isInfinite = true;
        Debug.Log("Infinite Sprint Started");

        yield return new WaitForSeconds(time);

        sprint.isInfinite = false;
        Debug.Log("Infinite Sprint Finished");
        yield break;
    }

    public void DropItem()
    {
        SpawnDropItem();
        UI_Inventory.instance.RefreshInventory();
        Debug.Log("Item Droped");
    }

    void SpawnDropItem()
    {
        foreach (SetItem item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == UI_SelectItem.index)
            {
                if (item.amount == 1) inventory.inventory.RemoveAt(UI_SelectItem.index);
                else item.amount--;
                GameObject itemSpawned = Instantiate(itemPrefab, cameraTransform.position + cameraTransform.forward * 1.5f, cameraTransform.rotation);
                itemSpawned.GetComponent<SetItem>().item = item.item;
                itemSpawned.GetComponent<Rigidbody>().AddForce(cameraTransform.forward * 5, ForceMode.VelocityChange);
                break;
            }
        }
    }
}