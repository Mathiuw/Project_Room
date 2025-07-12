using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(CanvasGroup))]
public class UI_Consumables : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Inventory inventory;
    [SerializeField] Image selectedConsumableImage;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();

        if (inventory)
        {
            DrawConsumableInventory();

            inventory.OnItemAdded += DrawConsumableInventory;
            inventory.OnItemRemoved += DrawConsumableInventory;
            inventory.OnConsumableIndexUpdate += DrawConsumableInventory;
        }
        else 
        {
            Debug.LogError("Cant find inventory");
            canvasGroup.alpha = 0;
            enabled = false;
            return;
        }
    }

    private void OnDisable()
    {
        if (inventory)
        {
            inventory.OnItemAdded -= DrawConsumableInventory;
            inventory.OnItemRemoved -= DrawConsumableInventory;
            inventory.OnConsumableIndexUpdate += DrawConsumableInventory;
        }
    }

    private void DrawConsumableInventory() 
    {
        if (inventory.InventoryList.Count == 0 || inventory.consumableIndexes.Count == 0)
        {
            canvasGroup.alpha = 0;
            return;
        }
        else
        {
            canvasGroup.alpha = 1;
            selectedConsumableImage.sprite = inventory.InventoryList[inventory.consumableIndexes[inventory.selectedConsumableIndex]].SOItem.hotbarSprite;
        }

        Debug.Log("Draw Consumable inventory");
    }
}