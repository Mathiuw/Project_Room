using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class UI_Inventory : MonoBehaviour
{
    [Header("Margin")]
    [SerializeField] float uiMargin = 95;

    [Header("Transforms")]
    [SerializeField] RectTransform hotbarTransform;
    [SerializeField] RectTransform slotsTransform;
    [SerializeField] RectTransform itemsTransform;

    [Header("Sprites")]
    [SerializeField] RectTransform slotSprite;
    [SerializeField] RectTransform itemSprite;

    [SerializeField] RectTransform[] uiSlots;
    [SerializeField] RectTransform[] items;

    [Header("Dynamic Move")]
    [SerializeField] float time = 0.25f;

    Inventory inventory;

    public Action<Inventory> OnInventoryRefresh;

    void Start() 
    {       
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            //Get inventory
            inventory = player.GetComponent<Inventory>();
            SetInventorySlots();

            //Set inventory events
            inventory.OnItemAdded += RefreshInventory;
            inventory.OnItemRemoved += RefreshInventory;

            //Refresh inventory when player start reloading
            player.GetComponent<PlayerWeaponInteraction>().onReloadStart += OnReloadStartFunc;

        }
        else Debug.LogError("Cant find Player");
    }

    void OnReloadStartFunc(float duration) 
    {
        RefreshInventory();
    }

    void SetInventorySlots() 
    {
        if (inventory == null) 
        {
            Debug.LogError("Cant set UI inventorty, inventory is NULL");
            return;
        }

        float uiSlotOffset = 0;

        uiSlots = new RectTransform[inventory.GetInventorySize()];
        items = new RectTransform[inventory.GetInventorySize()];

        //Instantiate all inventory slots
        for (int i = 0; i < inventory.GetInventorySize(); i++)
        {
            RectTransform slot = Instantiate(slotSprite, Vector2.zero, slotSprite.rotation, slotsTransform);
            slot.anchoredPosition = new Vector2(0 + uiSlotOffset, 0);
            uiSlotOffset += uiMargin;
            uiSlots[i] = slot;
        }

        //First inventory refresh
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        if (inventory == null)
        {
            Debug.LogError("Cant set UI inventorty, inventory is NULL");
            return;
        }

        //Destroy all UI items
        foreach (RectTransform item in items) if (item != null) Destroy(item.gameObject);

        //Rebuild all UI item
        for (int i = 0; i < inventory.GetInventorySize(); i++)
        {
            foreach (Item itemComp in inventory.inventoryList)
            {
                if (inventory.inventoryList.IndexOf(itemComp) == i)
                {
                    RectTransform item = Instantiate(itemSprite, uiSlots[i].position, Quaternion.identity, itemsTransform);
                    item.sizeDelta = new Vector2(90, 90);
                    item.GetComponentInChildren<Image>().sprite = itemComp.SOItem.hotbarSprite;
                    item.GetComponentInChildren<TextMeshProUGUI>().SetText(itemComp.amount.ToString());

                    if (itemComp.amount <= 1) item.transform.Find("Text_image").transform.gameObject.SetActive(false);
                    items[i] = item;

                    break;
                }
            }
        }

        //Check if inventory should move
        CheckIfMoveInventory();

        //Inventory refresh event
        OnInventoryRefresh?.Invoke(inventory);
    }

    IEnumerator lerpInventoryHeight(RectTransform rectTransform, float desiredHeight, float time)
    {
        float timeElapsed = 0;
        float percentageComplete = 0;

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 desiredPosition = new Vector2(hotbarTransform.anchoredPosition.x, desiredHeight);

        while (timeElapsed < time)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, desiredPosition, percentageComplete);

            timeElapsed += Time.deltaTime;
            percentageComplete = timeElapsed / time;

            yield return null;
        }
        rectTransform.anchoredPosition = desiredPosition;
    }

    void CheckIfMoveInventory()
    {
        StopAllCoroutines();

        if (inventory.inventoryList.Count == 0)
        {
            StartCoroutine(lerpInventoryHeight(hotbarTransform, 0, time));
        }
        else
        {
            StartCoroutine(lerpInventoryHeight(hotbarTransform, 100, time));
        }
    }
}