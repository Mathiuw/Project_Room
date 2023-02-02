using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    public static UI_Inventory instance;

    [Header("Margin")]
    [SerializeField] float uiMargin = 95;
    private float uiSlotOffset = 0;

    [Header("Parents")]
    [SerializeField] Transform SlotContainer;
    [SerializeField] Transform itemContainer;

    [Header("Sprites")]
    [SerializeField] GameObject uiSlotSprite;
    [SerializeField] GameObject itemSprite;

    [Header("Arrays")]
    [SerializeField] GameObject[] uiSlots;
    [SerializeField] GameObject[] items;

    [Header("Show ammo in UI")]
    [SerializeField] TextMeshProUGUI ammoUI;

    Inventory inventory;

    void Awake() => instance = this;

    void Start() 
    {
        if ((inventory = Player.instance.GetComponentInChildren<Inventory>()) && inventory == null)
        {
            Debug.LogError("Cant Find Player Inventory");
            return;
        }
    
        SetInventory(inventory);
    }

    public void SetInventory(Inventory inventory) 
    {
        uiSlots = new GameObject[inventory.InventorySize];
        items = new GameObject[inventory.InventorySize];

        for (int i = 0; i < inventory.InventorySize; i++)
        {
            GameObject slot = Instantiate(uiSlotSprite, Vector2.zero, Quaternion.identity, SlotContainer);
            RectTransform spriteTransform = slot.GetComponent<RectTransform>();
            spriteTransform.anchoredPosition = new Vector2(0 + uiSlotOffset, 0);
            uiSlotOffset += uiMargin;
            uiSlots[i] = slot;
        }

        this.inventory = inventory;

        RefreshInventory();
    }

    public void RefreshInventory()
    {
        foreach (GameObject item in items) Destroy(item);

        for (int i = 0; i < inventory.InventorySize; i++)
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

    public void ShowAmmoInUI(WeaponInteraction weaponInteraction)
    {
        if (!weaponInteraction.isHoldingWeapon) return;

        ShootGun shootGun = weaponInteraction.currentWeapon.shootGun;

        ammoUI.SetText(shootGun.ammo.ToString() + "/" + shootGun.maximumAmmo.ToString());
    }
}