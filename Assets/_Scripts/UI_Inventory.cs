using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    public static UI_Inventory instance;

    [Header("Margin")]
    [SerializeField] float uiMargin = 95;

    [Header("Parents")]
    [SerializeField] RectTransform slotsTransform;
    [SerializeField] RectTransform itemsTransform;

    [Header("Sprites")]
    [SerializeField] RectTransform slotSprite;
    [SerializeField] RectTransform itemSprite;

    [SerializeField] RectTransform[] uiSlots;
    [SerializeField] RectTransform[] items;

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

    void SetInventory(Inventory inventory) 
    {
        float uiSlotOffset = 0;

        uiSlots = new RectTransform[inventory.InventorySize];
        items = new RectTransform[inventory.InventorySize];

        for (int i = 0; i < inventory.InventorySize; i++)
        {
            RectTransform slot = Instantiate(slotSprite, Vector2.zero, slotSprite.rotation, slotsTransform);
            slot.anchoredPosition = new Vector2(0 + uiSlotOffset, 0);
            uiSlotOffset += uiMargin;
            uiSlots[i] = slot;
        }

        RefreshInventory();
    }

    public void RefreshInventory()
    {
        foreach (RectTransform item in items) if (item != null) Destroy(item.gameObject);

        for (int i = 0; i < inventory.InventorySize; i++)
        {
            foreach (SetItem itemComp in inventory.inventory)
            {
                if (inventory.inventory.IndexOf(itemComp) == i)
                {
                    RectTransform item = Instantiate(itemSprite, uiSlots[i].position, Quaternion.identity, itemsTransform);
                    item.sizeDelta = new Vector2(90, 90);
                    item.GetComponentInChildren<Image>().sprite = itemComp.item.hotbarSprite;
                    item.GetComponentInChildren<TextMeshProUGUI>().SetText(itemComp.amount.ToString());

                    if (itemComp.amount <= 1) item.transform.Find("Text_image").transform.gameObject.SetActive(false);
                    items[i] = item;

                    break;
                }
            }
        }
    }
}