using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAndDropItems : MonoBehaviour,ICanDo
{
    private bool canDo = true;

    [Header("Pickup item")]
    [SerializeField] private float rayLenght;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask itemMask;
    Inventory inventory;
    UI_Inventory uiInventory;

    [Header("Drop item")]
    [SerializeField] private GameObject itemPrefab;

    private void Awake()
    {
        GameObject playerRoot = transform.parent.gameObject;

        inventory = GetComponent<Inventory>();
        uiInventory = playerRoot.GetComponentInChildren<UI_Inventory>();

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    private void Update()
    {
        if (!canDo) return;
        DropItem();
        UseItem();
        pickupItem();
    }

    private void pickupItem()
    {
        RaycastHit hit;

        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLenght, itemMask))
        {
            if (hit.transform.GetComponent<SetItem>() && inventory.AddItem(hit.transform.GetComponent<SetItem>()))
            {
                uiInventory.RefreshInventory();
                Destroy(hit.transform.gameObject);
                Debug.Log("Picked item");
            }
        }
    }

    private void UseItem()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;

        foreach (SetItem item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == SelectItem.index && item.item.isConsumable)
            {
                Health.AddHealth(item.item.recoverHealth);

                if (item.item.canInfiniteSprint)
                {               
                    Sprint sprintScript = GetComponent<Sprint>();
                    sprintScript.infiniteSprintEvent += OnAdrenalineUsed;
                    sprintScript.InfiniteSprint(item.item.infiniteAdrenalineDuration);
                }

                if (item.amount > 1) item.amount--;
                else inventory.inventory.Remove(item);

                uiInventory.RefreshInventory();
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

    private void DropItem()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        SpawnDropItem();
        uiInventory.RefreshInventory();
        Debug.Log("Drop item");
    }

    private void SpawnDropItem()
    {
        foreach (SetItem item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == SelectItem.index)
            {
                if (item.amount == 1) inventory.inventory.RemoveAt(SelectItem.index);
                else item.amount--;
                GameObject itemSpawned = Instantiate(itemPrefab, cameraTransform.position + cameraTransform.forward * 1.5f, cameraTransform.rotation);
                itemSpawned.GetComponent<SetItem>().item = item.item;
                itemSpawned.GetComponent<Rigidbody>().AddForce(cameraTransform.forward * 5, ForceMode.VelocityChange);
                break;
            }
        }
    }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}