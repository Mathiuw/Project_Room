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
            DrawInventory();
        }
        else 
        {
            Debug.LogError("Cant find inventory");
            canvasGroup.alpha = 0;
            enabled = false;
            return;
        }

        inventory.OnConsumableListUpdate += DrawInventory;
    }

    private void OnDisable()
    {
        inventory.OnConsumableListUpdate -= DrawInventory;
    }

    private void DrawInventory() 
    {
        if (inventory.InventoryList.Count == 0 || inventory.consumableIndexes.Count == 0)
        {
            canvasGroup.alpha = 0;
            return;
        }
        else
        {
            canvasGroup.alpha = 1;
            selectedConsumableImage.sprite = inventory.InventoryList[inventory.selectedConsumableIndex].SOItem.hotbarSprite;
        }
    }
}