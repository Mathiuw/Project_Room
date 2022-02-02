using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private GameObject hotbarSlots;
    [SerializeField] private GameObject itemSelected;
    public static int index;

    private void Update()
    {
        ChangeSlot();
    }

    private void ChangeSlot()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            index++;
            if (index > hotbarSlots.transform.childCount - 1)
            {
                index = 0;
            }
            itemSelected.transform.GetComponent<RectTransform>().anchoredPosition = hotbarSlots.transform.GetChild(index).GetComponent<RectTransform>().anchoredPosition; 
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            index--;
            if (index < 0)
            {
                index = hotbarSlots.transform.childCount - 1;
            }
            itemSelected.transform.GetComponent<RectTransform>().anchoredPosition = hotbarSlots.transform.GetChild(index).GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
