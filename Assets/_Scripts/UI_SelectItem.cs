using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectItem : MonoBehaviour
{
    [SerializeField] GameObject hotbarSlots;
    [SerializeField] GameObject itemSelected;
    RectTransform itemSelectedTransform;
    public static int index;

    void Update() 
    {
        //Change Inventory Slot
        if (Input.mouseScrollDelta.y < 0) ChangeSlot(1);
        if (Input.mouseScrollDelta.y > 0) ChangeSlot(-1);
    }

    Vector2 HotbarPosition(int index) => hotbarSlots.transform.GetChild(index).GetComponent<RectTransform>().anchoredPosition;

    public void ChangeSlot(int addIndex)
    {
        itemSelectedTransform = itemSelected.GetComponent<RectTransform>();
        index += addIndex;
        if (index > hotbarSlots.transform.childCount - 1) index = 0;
        else if (index < 0) index = hotbarSlots.transform.childCount - 1;
        itemSelectedTransform.anchoredPosition = HotbarPosition(index);
    }
}
