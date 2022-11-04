using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private GameObject hotbarSlots;
    [SerializeField] private GameObject itemSelected;
    RectTransform itemSelectedTransform;
    public static int index;

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
