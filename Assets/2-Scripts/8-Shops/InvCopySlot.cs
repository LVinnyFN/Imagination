using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvCopySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Inventory inventory;
    public GameObject selectedborder;

    public void OnPointerEnter(PointerEventData eventdata)
    {
        if (inventory.transform.GetChild(transform.GetSiblingIndex()).GetComponent<Slot>().item != null)
        {
            if (inventory.transform.GetChild(transform.GetSiblingIndex()).GetComponent<Slot>().item is Equipable)
            {
                Equipable myitem = (Equipable)inventory.transform.GetChild(transform.GetSiblingIndex()).GetComponent<Slot>().item;
                GameController.controller.uiController.ShowEquipableTooltip(true, myitem);
            }
            else
            {
                CraftMat myitem = (CraftMat)inventory.transform.GetChild(transform.GetSiblingIndex()).GetComponent<Slot>().item;
                GameController.controller.uiController.ShowCraftMatTooltip(true, myitem);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        GameController.controller.uiController.ShowEquipableTooltip(false, null);
        GameController.controller.uiController.ShowCraftMatTooltip(false, null);
    }

    public void ShowSelectedBorder()
    {
        if (inventory.transform.GetChild(transform.GetSiblingIndex()).GetComponent<Slot>().item != null)
        {
            transform.parent.GetComponent<InvCopyGrid>().ResetSelected();
            selectedborder.SetActive(true);
        }
    }

    
}
