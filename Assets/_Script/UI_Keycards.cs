using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Keycards : MonoBehaviour
{
    [field: SerializeField] public float spriteOfset { get; private set; } = 10f;
    Inventory inventory;
    Image spriteTemplate;

    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform player = GameObject.FindWithTag("Player").transform;

        if (!player)
        {
            Debug.LogError("Cant find player");
            return;
        }

        inventory = player.GetComponent<Inventory>();

        if (!inventory)
        {
            Debug.LogError("Cant find inventory");
            return;
        }

        spriteTemplate = GetComponentInChildren<Image>();

        if (!spriteTemplate)
        {
            Debug.LogError("Cant find spriteTemplate image");
            return;
        }

        inventory.OnItemAdded += DrawKeycards;
        inventory.OnItemRemoved += DrawKeycards;

        DrawKeycards();
    }

    private void OnDisable()
    {
        inventory.OnItemAdded -= DrawKeycards;
        inventory.OnItemRemoved -= DrawKeycards;
    }

    private void DrawKeycards() 
    {
        if (inventory.InventoryList.Count == 0)
        {
            canvasGroup.alpha = 0;
            return;
        }

        int keycardcount = 0;
        float offfset = 0f;
        bool firstSprite = false;

        for (int i = 0; i < inventory.InventoryList.Count; i++)
        {
            if (inventory.InventoryList[i].SOItem.GetType() == typeof(SOKeycard))
            {
                if (!firstSprite)
                {
                    Image keycardSprite = Instantiate(spriteTemplate, new Vector3(spriteTemplate.rectTransform.anchoredPosition.x + offfset, 
                        spriteTemplate.rectTransform.anchoredPosition.y), Quaternion.identity);                            
                    keycardSprite.sprite = inventory.InventoryList[i].SOItem.hotbarSprite;
                }
                else
                {
                    spriteTemplate.sprite = inventory.InventoryList[i].SOItem.hotbarSprite;
                    offfset += spriteOfset;
                    firstSprite = true;
                }

                offfset += spriteOfset;
                keycardcount++;
            }
        }

        if (keycardcount != 0)
        {
            canvasGroup.alpha = 1;
        }
        else canvasGroup.alpha = 0;

        Debug.Log("Draw keycard inventory");
    }
}
