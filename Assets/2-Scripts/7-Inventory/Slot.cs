using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Microsoft.Win32.SafeHandles;
using TMPro;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemAmountText;
    public Inventory inventory;

    public Item item;
    public int itemAmount;
    public Sprite initialimage;

    public Drag dragobj;
    public int amountaux;
    public GameObject unequipFeedback;

    void Start()
    {
        UpdateUI();
    }

    public void OnPointerEnter(PointerEventData eventdata)
    {
        if (item != null)
        {
            if (item is Equipable)
            {
                Equipable myitem = (Equipable)item;
                GameController.controller.uiController.ShowEquipableTooltip(true, myitem);
                inventory.GlowSlot(myitem, true);
            }
            else
            {
                CraftMat myitem = (CraftMat)item;
                GameController.controller.uiController.ShowCraftMatTooltip(true, myitem);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        if (item is Equipable)
        {
            Equipable myitem = (Equipable)item;
            inventory.GlowSlot(myitem, false);
        }
        GameController.controller.uiController.ShowEquipableTooltip(false, null);
        GameController.controller.uiController.ShowCraftMatTooltip(false, null);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            dragobj.StartDrag(item, this.gameObject);
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
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
        if (dragobj.isdragging)
        {
            dragobj.dropped = true;
            //Se não tem item nesse slot
            if (item == null)
            {
                //Se trouxemos alguma coisa no drag
                if (dragobj.dragitem != null)
                {
                    //Se veio de um Slot de Equip (e é um equipavel)
                    if (dragobj.originslot.GetComponent<EquipSlot>() && dragobj.dragitem is Equipable)
                    {
                        Equipable myequip = (Equipable)dragobj.dragitem;
                        //Se é minha espada ou escudo
                        if (myequip.type == "Weapon" || myequip.type == "Shield")
                        {
                            dragobj.originslot.GetComponent<Image>().color = Color.white;
                            StartCoroutine(UnequipFeedback());
                            dragobj.dropped = false;
                        }
                        //Se não é a espada ou escudo (eu deixo tirar)
                        else
                        {
                            AddItem(dragobj.dragitem, dragobj.dragamount);
                        }
                    }
                    else
                    {
                        AddItem(dragobj.dragitem, dragobj.dragamount);
                    }
                }
                dragobj.StopDrag();
            }
            //Já tem item nesse slot
            else
            {
                //Salva o que temos aqui no auxiliar
                Item itemaux = item;
                //Se for um equipamento que tem no slot
                if (item is Equipable)
                {
                    Equipable equipaxu2 = (Equipable)item;
                    //Se veio dos equipados
                    if (dragobj.originslot.GetComponent<EquipSlot>())
                    {
                        Equipable equipaux = (Equipable)dragobj.dragitem;
                        //Se são do mesmo tipo (faz a troca)
                        if (equipaux.type == equipaxu2.type)
                        {
                            Item itemaux2 = dragobj.dragitem;
                            dragobj.StopDrag();
                            inventory.selectedSlot = this; //Seleciona esse slot
                            dragobj.originslot.GetComponent<EquipSlot>().EquipItem(); // Manda o slot de equip equipar o item desse slot
                            AddItem(itemaux2, 1);
                        }
                        //Se não, não troca
                        else
                        {
                            dragobj.originslot.GetComponent<Image>().color = Color.white;
                            dragobj.dropped = false;
                            dragobj.StopDrag();
                        }

                    }
                    //Se não veio dos equipados
                    else
                    {
                        amountaux = 1;
                        itemAmount = 0;
                        AddItem(dragobj.dragitem, dragobj.dragamount);
                        dragobj.StopDrag();
                        dragobj.originslot.GetComponent<Slot>().AddItem(itemaux, amountaux);
                    }
                }
                //Se for material que tem no slot
                else
                {
                    CraftMat mataux = (CraftMat)item;
                    amountaux = itemAmount;
                    //Se veio dos equipados
                    if (dragobj.originslot.GetComponent<EquipSlot>())
                    {
                        dragobj.originslot.GetComponent<Image>().color = Color.white;
                        dragobj.dropped = false;
                        dragobj.StopDrag();
                    }
                    else
                    {
                        if (item.itemName != dragobj.dragitem.itemName) // arrastando para onde tem materiail diferente
                        {
                            RemoveItem(itemAmount);
                            AddItem(dragobj.dragitem, dragobj.dragamount);
                            dragobj.StopDrag();
                            dragobj.originslot.GetComponent<Slot>().AddItem(itemaux, amountaux);
                        }
                        else if (item.itemName == dragobj.dragitem.itemName && itemAmount + dragobj.dragamount <= mataux.maxstackamount)
                        {
                            AddItem(dragobj.dragitem, dragobj.dragamount);
                            dragobj.StopDrag();
                        }
                        else
                        {
                            int spare = mataux.maxstackamount - itemAmount;
                            AddItem(dragobj.dragitem, spare);
                            int leftover = dragobj.dragamount - spare;
                            dragobj.StopDrag();
                            dragobj.originslot.GetComponent<Slot>().AddItem(itemaux, leftover);
                        }
                    }
                }
            }
        }
    }

    public IEnumerator UnequipFeedback()
    {
        unequipFeedback.SetActive(true);
        yield return new WaitForSeconds(2f);
        unequipFeedback.SetActive(false);
    }

    //Adds an item to the Slot.
    public void AddItem(Item itemToAdd, int amount)
    {
        if (item != null && item.itemName == itemToAdd.itemName && itemToAdd is CraftMat)
        {
            itemAmount += amount;
        }
        else
        {
            item = itemToAdd;
            itemAmount = amount;
        }
        UpdateUI();
    }

    //Removes an item from the Slot.
    public void RemoveItem(int amount)
    {
        if (item != null)
        {
            itemAmount -= amount;
            if (itemAmount <= 0)
            {
                item = null;
                GetComponent<Image>().sprite = initialimage;
                transform.GetChild(1).GetComponent<Image>().sprite = null;
                transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
        }
        UpdateUI();
        //inventory.CheckPickUpQuests(); // funcionaria para o else
    }

    //Updates the Slot UI.
    void UpdateUI()
    {
        if (item != null)
        {
            if (item is Equipable)
            {
                itemAmountText.gameObject.SetActive(false);
                Equipable myitem = (Equipable)item;
                transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[myitem.tier - 1];
                transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            }
            else
            {
                itemAmountText.gameObject.SetActive(true);
                itemAmountText.text = itemAmount.ToString();
            }
            itemIcon.sprite = item.itemIcon;
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else
        {
            itemIcon.sprite = initialimage;
            itemAmountText.gameObject.SetActive(false);
        }
    }
}
