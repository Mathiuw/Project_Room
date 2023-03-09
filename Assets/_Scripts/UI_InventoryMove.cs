using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UI_Inventory))]
public class UI_InventoryMove : MonoBehaviour
{
    [SerializeField] RectTransform hotbarTransform;
    [SerializeField] float time;

    void Start() 
    {
        GetComponent<UI_Inventory>().onInventoryChange += MoveInventory;
    }

    IEnumerator lerpInventoryHeight(RectTransform rectTransform, float desiredHeight , float time) 
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

    void MoveInventory(Inventory inventory) 
    {
        if (inventory.inventory.Count == 0)
        {
            StopAllCoroutines();
            StartCoroutine(lerpInventoryHeight(hotbarTransform, 0, time));
        }
        else 
        {
            StopAllCoroutines();
            StartCoroutine(lerpInventoryHeight(hotbarTransform, 100, time));
        } 
    }
}
