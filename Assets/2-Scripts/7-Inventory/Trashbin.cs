using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Trashbin : MonoBehaviour, IDropHandler
{
    public GameObject delconfirm;
    public Drag dragobj;

    public void OnDrop(PointerEventData eventData)
    {
        if (dragobj.originslot.GetComponent<EquipSlot>() && dragobj.dragitem is Equipable)
        {
            Equipable myequip = (Equipable)dragobj.dragitem;
            //Se é minha espada ou escudo
            if (myequip.type == "Weapon" || myequip.type == "Shield")
            {
                dragobj.originslot.GetComponent<Image>().color = Color.white;
                dragobj.dropped = false;
            }
            else
            {
            delconfirm.SetActive(true);
            }
        }
        else
        {
            delconfirm.SetActive(true);
        }
    }

    public void Yes()
    {
        dragobj.dropped = true;
        dragobj.StopDrag();
    }

    public void No()
    {
        dragobj.originslot.GetComponent<Image>().color = Color.white;
        dragobj.StopDrag();
    }


}