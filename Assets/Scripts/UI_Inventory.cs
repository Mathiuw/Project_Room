using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;

    [Header("Margin")]
    [SerializeField] private float uiMargin = 95;
    private float uiSlotOffset = 0;

    [Header("Parents")]
    [SerializeField] private Transform SlotContainer;
    [SerializeField] private Transform itemContainer;

    [Header("Sprites")]
    [SerializeField] private GameObject uiSlotSprite;
    [SerializeField] private GameObject itemSprite;

    [Header("Arrays")]
    [SerializeField] private GameObject[] uiSlots;
    [SerializeField] private GameObject[] items;

    [Header("Show ammo in UI")]
    [SerializeField] GameObject gunHolder;
    [SerializeField] TextMeshProUGUI ammoUI;
    private void Awake()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();

        uiSlots = new GameObject[inventory.inventorySize];
        items = new GameObject[inventory.inventorySize];

        for (int i = 0; i < inventory.inventorySize; i++)
        {
            GameObject slot = Instantiate(uiSlotSprite, Vector2.zero, Quaternion.identity, SlotContainer);
            RectTransform spriteTransform = slot.GetComponent<RectTransform>();
            spriteTransform.anchoredPosition = new Vector2(0 + uiSlotOffset, 0);
            uiSlotOffset += uiMargin;
            uiSlots[i] = slot;
        }

        RefreshInventory();
    }

    private void Update()
    {
        ShowAmmoInUI();
    }

    public void RefreshInventory()
    {
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            foreach (SetItem itemComp in inventory.inventory)
            {
                if (inventory.inventory.IndexOf(itemComp) == i)
                {
                    GameObject item = Instantiate(itemSprite, uiSlots[i].transform.position, Quaternion.identity, itemContainer);
                    item.GetComponentInChildren<Image>().sprite = itemComp.item.hotbarSprite;
                    item.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(90,90);
                    item.GetComponentInChildren<TextMeshProUGUI>().SetText(itemComp.amount.ToString());
                    if (itemComp.amount <= 1)
                    {
                        item.transform.Find("Text_image").transform.gameObject.SetActive(false);
                    }
                    items[i] = item;
                    break;
                }
            }
        }
    }

    private void ShowAmmoInUI()
    {
        if (WeaponPickup.IsHoldingWeapon)
        {
            ShootGun gunScript = gunHolder.GetComponentInChildren<ShootGun>();
            ammoUI.SetText(gunScript.ammo.ToString() + "/" + gunScript.maximumAmmo.ToString());
        }
    }
}