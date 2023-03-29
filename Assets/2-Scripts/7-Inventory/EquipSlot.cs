using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private TutorialMush tutmush;

    [SerializeField] private Inventory inventory;
    private Image sloticon;
    public Equipable equip;
    public Sprite initialimage;

    public Drag dragobj;
    public Item itemaux = null;
    public Equipable equipaux;
    public GameObject sword;
    public GameObject shield;

    void Start()
    {
        sloticon = transform.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventdata)
    {
        if (equip != null)
        {
            GameController.controller.uiController.ShowEquipableTooltip(true, equip);
        }
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        GameController.controller.uiController.ShowEquipableTooltip(false, null);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equip != null)
        {
            Item itemaux = equip;
            dragobj.StartDrag(itemaux, this.gameObject);
            dragobj.originslot.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragobj.isdragging)
        {
            dragobj.dragimage.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragobj.isdragging) // not dropped like it should
        {
            GetComponent<Image>().color = Color.white;
            dragobj.isdragging = false;
            dragobj.dragimage.gameObject.SetActive(false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dragobj.isdragging && dragobj.dragitem is Equipable)
        {
            if (dragobj.dragitem != null)
            {
                equipaux = (Equipable)dragobj.dragitem;
            }
            if (equip != null) //se tiver item equipado aqui já faz um item auxiliar pra deixar isso salvo
            {
                itemaux = equip;
            }

            // Primeira vez equipando sword (TUTORIAL)
            if (equipaux != null)
            {
                if (equip == null && equipaux.type == "Weapon" && this.transform.GetSiblingIndex() == 0)
                {
                    sword.SetActive(true);
                    if (shield.activeSelf)
                    {
                        tutmush.equipped = true;
                        tutmush.exclamation.SetActive(true);
                    }
                }
                // Primeira vez equipando shield (TUTORIAL)
                if (equip == null && equipaux.type == "Shield" && this.transform.GetSiblingIndex() == 1)
                {
                    shield.SetActive(true);
                    if (sword.activeSelf)
                    {
                        tutmush.equipped = true;
                        tutmush.exclamation.SetActive(true);
                    }
                }
                //Verifica se o que ta sendo arrastado combina com esse slot
                if ((equipaux.type == "Weapon" && this.transform.GetSiblingIndex() == 0) ||
                    (equipaux.type == "Shield" && this.transform.GetSiblingIndex() == 1) ||
                    (equipaux.type == "Head" && this.transform.GetSiblingIndex() == 2) ||
                    (equipaux.type == "Body" && this.transform.GetSiblingIndex() == 3) ||
                    (equipaux.type == "Foot" && this.transform.GetSiblingIndex() == 4))
                {
                    //verifica se veio do inventario
                    if (dragobj.originslot.GetComponent<Slot>())
                    {
                        Debug.Log("Ta certo! Sibling: " + this.transform.GetSiblingIndex());
                        dragobj.dropped = true;
                        inventory.selectedSlot = dragobj.originslot.GetComponent<Slot>();
                        EquipItem();
                    }
                    //se não veio do inventario, não quero
                    else
                    {
                        dragobj.originslot.GetComponent<Image>().color = Color.white;
                    }
                }
                //se não combina com esse slot, não quero
                else
                {
                    dragobj.originslot.GetComponent<Image>().color = Color.white;
                    Debug.Log("Ta errado! Sibling: " + this.transform.GetSiblingIndex());
                }
            }
            //Limpa o drag
            dragobj.StopDrag();

            //Verifica se tinha item aqui e o que trouxe era do mesmo tipo
            if (itemaux != null && equipaux.type == equip.type)
            {
                //Verifica se veio do inventário
                if (dragobj.originslot.GetComponent<Slot>())
                {
                    dragobj.originslot.GetComponent<Slot>().AddItem(itemaux, 1);
                    itemaux = null;
                }
            }
        }
        
    }

    public void EquipItem()
    {
        if (inventory.selectedSlot != null)
        {
            Equipable itemtoEquip = (Equipable)inventory.selectedSlot.item;
            if (equip == null)
            {
                equip = itemtoEquip;
                inventory.selectedSlot.item = null;
                inventory.selectedSlot.GetComponent<Image>().sprite = inventory.selectedSlot.initialimage;
                inventory.selectedSlot.transform.GetChild(1).GetComponent<Image>().sprite = null;
                inventory.selectedSlot.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                EquipStats();
            }
            else
            {
                Equipable aux = equip;
                UnequipStats();
                equip = itemtoEquip;
                EquipStats();
                inventory.selectedSlot.AddItem(aux, 1);
            }
        }
    }

    public void UnequipItem()
    {
        GetComponent<Image>().sprite = initialimage;
        transform.GetChild(0).GetComponent<Image>().sprite = null;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        UnequipStats();
        equip = null;
    }

    public void EquipStats()
    {
        if (equip != null)
        {
            EquipMain();
            EquipPrimary();
            if (equip.tier >= 2)
            {
                EquipSecondary1();
            }
            if (equip.tier >= 3)
            {
                EquipSecondary2();
            }
        }
        GetComponent<Image>().sprite = equip.itemIcon;
        transform.GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[equip.tier - 1];
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        GameController.controller.player.GetComponent<Player>().RefreshStatsUI();
    }

    public void UnequipStats()
    {
        if (equip != null)
        {
            UnequipMain();
            UnequipPrimary();
        }
        if (equip.tier >= 2)
        {
            UnequipSecondary1();
        }
        if (equip.tier >= 3)
        {
            UnequipSecondary2();
        }
    }


    private void EquipMain()
    {
        switch (equip.type)
        {
            case "Weapon":
                GameController.controller.player.GetComponent<Player>().RefreshStat("meleeDmg", equip.mainValue);
                break;
            default:
                GameController.controller.player.GetComponent<Player>().RefreshStat("armor", equip.mainValue);
                break;
        }
    }

    private void UnequipMain()
    {
        switch (equip.type)
        {
            case "Weapon":
                GameController.controller.player.GetComponent<Player>().RefreshStat("meleeDmg", -equip.mainValue);
                break;
            default:
                GameController.controller.player.GetComponent<Player>().RefreshStat("armor", -equip.mainValue);
                break;
        }
    }

    private void EquipPrimary()
    {
        switch (equip.primaryAtribute)
        {
            case "Str":
                GameController.controller.player.GetComponent<Player>().RefreshStat("strength", equip.primaryValue);
                break;
            case "Agi":
                GameController.controller.player.GetComponent<Player>().RefreshStat("agility", equip.primaryValue);
                break;
            case "Int":
                GameController.controller.player.GetComponent<Player>().RefreshStat("intelligence", equip.primaryValue);
                break;
            case "Vit":
                GameController.controller.player.GetComponent<Player>().RefreshStat("vitality", equip.primaryValue);
                break;
            default:
                break;
        }
    }
    private void UnequipPrimary()
    {
        switch (equip.primaryAtribute)
        {
            case "Str":
                GameController.controller.player.GetComponent<Player>().RefreshStat("strength", -equip.primaryValue);
                break;
            case "Agi":
                GameController.controller.player.GetComponent<Player>().RefreshStat("agility", -equip.primaryValue);
                break;
            case "Int":
                GameController.controller.player.GetComponent<Player>().RefreshStat("intelligence", -equip.primaryValue);
                break;
            case "Vit":
                GameController.controller.player.GetComponent<Player>().RefreshStat("vitality", -equip.primaryValue);
                break;
            default:
                break;
        }
    }

    private void EquipSecondary1()
    {
        switch (equip.secondaryAtribute1)
        {
            case "PExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("meleeDmg", equip.secondaryValue1);
                break;
            case "AExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("armor", equip.secondaryValue1);
                break;
            case "Life":
                GameController.controller.player.GetComponent<Player>().RefreshStat("life", equip.secondaryValue1);
                break;
            case "CritDmg":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critDmg", equip.secondaryValue1);
                break;
            case "CritChance":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critChance", equip.secondaryValue1);
                break;
            default:
                break;
        }
    }
    private void UnequipSecondary1()
    {
        switch (equip.secondaryAtribute1)
        {
            case "PExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("meleeDmg", -equip.secondaryValue1);
                break;
            case "AExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("armor", -equip.secondaryValue1);
                break;
            case "Life":
                GameController.controller.player.GetComponent<Player>().RefreshStat("life", -equip.secondaryValue1);
                break;
            case "CritDmg":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critDmg", -equip.secondaryValue1);
                break;
            case "CritChance":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critChance", -equip.secondaryValue1);
                break;
            default:
                break;
        }
    }

    private void EquipSecondary2()
    {
        switch (equip.secondaryAtribute2)
        {
            case "PExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("meleeDmg", equip.secondayValue2);
                break;
            case "AExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("armor", equip.secondayValue2);
                break;
            case "Life":
                GameController.controller.player.GetComponent<Player>().RefreshStat("life", equip.secondayValue2);
                break;
            case "CritDmg":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critDmg", equip.secondayValue2);
                break;
            case "CritChance":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critChance", equip.secondayValue2);
                break;
            default:
                break;
        }
    }

    private void UnequipSecondary2()
    {
        switch (equip.secondaryAtribute2)
        {
            case "PExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("meleeDmg", -equip.secondayValue2);
                break;
            case "AExtra":
                GameController.controller.player.GetComponent<Player>().RefreshStat("armor", -equip.secondayValue2);
                break;
            case "Life":
                GameController.controller.player.GetComponent<Player>().RefreshStat("life", -equip.secondayValue2);
                break;
            case "CritDmg":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critDmg", -equip.secondayValue2);
                break;
            case "CritChance":
                GameController.controller.player.GetComponent<Player>().RefreshStat("critChance", -equip.secondayValue2);
                break;
            default:
                break;
        }
    }


}
