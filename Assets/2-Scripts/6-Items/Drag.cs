using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drag : MonoBehaviour
{
    public Item dragitem = null;
    public Image dragimage;
    public int dragamount;
    public bool isdragging = false;
    public bool dropped = false;
    public GameObject originslot=null;
    public Image dragborder;

    public void StartDrag(Item item, GameObject slotref)
    {
        originslot = slotref;
        dragitem = item;
        dragimage.gameObject.SetActive(true);
        dragimage.sprite = item.itemIcon;
        isdragging = true;
        if (item is Equipable)
        {
            dragamount = 1;
        }
        else
        {
            CraftMat itemaux = (CraftMat)item;
            dragamount = originslot.GetComponent<Slot>().itemAmount;
        }
    }

    public void StopDrag()
    {
        if (dropped)
        {
            if (originslot.GetComponent<EquipSlot>())
            {
                originslot.GetComponent<EquipSlot>().UnequipItem();
                originslot.GetComponent<Image>().color = Color.white;
            }
            else
            {
                originslot.GetComponent<Slot>().RemoveItem(dragamount);
                originslot.GetComponent<Image>().color = new Color(1,1,1);
            }
            
            dropped = false;
        }

        isdragging = false;
        dragimage.gameObject.SetActive(false);
        dragitem = null;
        dragamount = 0;
    }
}
