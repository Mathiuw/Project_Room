using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
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
    [SerializeField] TextMeshProUGUI ammoUI;

    void Start() 
    {
        uiSlots = new GameObject[Player.Instance.Inventory.InventorySize];
        items = new GameObject[Player.Instance.Inventory.InventorySize];

        for (int i = 0; i < Player.Instance.Inventory.InventorySize; i++)
        {
            GameObject slot = Instantiate(uiSlotSprite, Vector2.zero, Quaternion.identity, SlotContainer);
            RectTransform spriteTransform = slot.GetComponent<RectTransform>();
            spriteTransform.anchoredPosition = new Vector2(0 + uiSlotOffset, 0);
            uiSlotOffset += uiMargin;
            uiSlots[i] = slot;
        }

        RefreshInventory();
    }

    void Update() {  ShowAmmoInUI(); }

    public void RefreshInventory()
    {
        foreach (GameObject item in items) Destroy(item);

        for (int i = 0; i < Player.Instance.Inventory.InventorySize; i++)
        {
            foreach (SetItem itemComp in Player.Instance.Inventory.inventory)
            {
                if (Player.Instance.Inventory.inventory.IndexOf(itemComp) == i)
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

    void ShowAmmoInUI()
    {
        if (!Player.Instance.WeaponInteraction.IsholdingWeapon()) return;

        ShootGun shootGun = Player.Instance.GetPlayerGun();
        ammoUI.SetText(shootGun.ammo.ToString() + "/" + shootGun.maximumAmmo.ToString());
    }
}