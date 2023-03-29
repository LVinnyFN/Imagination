using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int goldAmount;
    public Slot selectedSlot;
    public Text blacksellingpricetext;
    public Text shopsellingpricetext;
    public Player player;
    [SerializeField] private GameObject equipButton;

    public EquipSlot weaponslot;
    public EquipSlot shieldslot;
    public EquipSlot headslot;
    public EquipSlot bodyslot;
    public EquipSlot footslot;
    [SerializeField] private GameObject glowslot;

    public void SelectSlotCopy(int index)
    {
        selectedSlot = transform.GetChild(index).GetComponent<Slot>();
        if (GameController.controller.uiController.shopMenu != null)
        {
            if (GameController.controller.uiController.shopMenu.CompareTag("Blacksmith"))
            {
                blacksellingpricetext.text = (selectedSlot.item.price / 2).ToString();
            }
            else
            {
                shopsellingpricetext.text = (selectedSlot.item.price / 2).ToString();
            }
        }
    }

    public void ShowEquipeItemButton()
    {
        if (selectedSlot.item is Equipable)
        {
            equipButton.SetActive(true);
        }
        else
        {
            equipButton.SetActive(false);
        }
    }

    public void EquipItem()
    {
        if (selectedSlot != null && selectedSlot.item != null && selectedSlot.item is Equipable)
        {
            Equipable itemtoEquip = (Equipable)selectedSlot.item;
            switch (itemtoEquip.type)
            {
                case "Weapon":
                    weaponslot.EquipItem();
                    break;
                case "Shield":
                    shieldslot.EquipItem();
                    break;
                case "Head":
                    headslot.EquipItem();
                    break;
                case "Body":
                    bodyslot.EquipItem();
                    break;
                case "Foot":
                    footslot.EquipItem();
                    break;
                default:
                    break;
            }
        }
    }

    public void GlowSlot(Equipable item, bool status)
    {
        switch (item.type)
        {
            case "Weapon":
                glowslot.transform.GetChild(0).gameObject.SetActive(status);
                break;
            case "Shield":
                glowslot.transform.GetChild(1).gameObject.SetActive(status);
                break;
            case "Head":
                glowslot.transform.GetChild(2).gameObject.SetActive(status);
                break;
            case "Body":
                glowslot.transform.GetChild(3).gameObject.SetActive(status);
                break;
            case "Foot":
                glowslot.transform.GetChild(4).gameObject.SetActive(status);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Add an item to player's inventory
    /// </summary>
    /// <param name="itemToAdd">Item to be added</param>
    /// <param name="amount">Amount of that item to be added</param>
    public bool AddItem(Item itemToAdd, int amount)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot currentSlot = transform.GetChild(i).GetComponent<Slot>();
            if (currentSlot.item != null && currentSlot.item.itemName == itemToAdd.itemName && itemToAdd is CraftMat)
            {
                CraftMat mattoadd = (CraftMat)itemToAdd;
                if (currentSlot.itemAmount + amount <= mattoadd.maxstackamount)
                {
                    currentSlot.AddItem(mattoadd, amount);
                    if (GameController.controller.uiController.itempicked.activeSelf)
                    {
                        GameController.controller.uiController.Item2PickedFeedback(itemToAdd);
                    }
                    else
                    {
                        GameController.controller.uiController.ItemPickedFeedback(itemToAdd);
                    }
                    CheckPickUpQuests();
                    return true;
                }
            }
            else if (currentSlot.item == null)
            {
                currentSlot.AddItem(itemToAdd, amount);
                if (GameController.controller.uiController.itempicked.activeSelf)
                {
                    GameController.controller.uiController.Item2PickedFeedback(itemToAdd);
                }
                else
                {
                    GameController.controller.uiController.ItemPickedFeedback(itemToAdd);
                }
                return true;
            }

        }
        Debug.Log("Inventory is Full!");
        return false;
    }

    public void CheckPickUpQuests()
    {
        Element aux = GameController.controller.uiController.GetQuestLog().GetComponent<QuestLog>().myQuests.first.next;

        while (aux != null)
        {
            Quest auxDoAux = (Quest)aux.myContent;
            if (auxDoAux.type.Equals("pickup") && !auxDoAux.isCompleted)
            {
                if (auxDoAux.materialtopickup1 != "")
                {
                    if (CheckSpecificMat(auxDoAux.materialtopickup1, auxDoAux.quantitytopickup1))
                    {
                        if (auxDoAux.materialtopickup2 != "")
                        {
                            if (CheckSpecificMat(auxDoAux.materialtopickup2, auxDoAux.quantitytopickup2))
                            {
                                auxDoAux.CheckPickUp();
                            }
                            else
                            {
                                //auxDoAux.isCompleted = false;
                                // Se vender material tem que fazer o Uncomplete aqui dentro
                            }
                        }
                        else
                        {
                            auxDoAux.CheckPickUp();
                        }
                    }
                    else
                    {
                        //auxDoAux.isCompleted = false;
                        // Se vender material tem que fazer o Uncomplete aqui dentro
                    }
                }
            }
            aux = aux.next;
        }
    }

    public void SellItem()
    {
        if (selectedSlot != null && selectedSlot.item != null) //o botão só funciona se tiver selecionado um slot e se o slot não estiver vazio
        {
            goldAmount += selectedSlot.item.price / 2;
            if (selectedSlot.itemAmount == 1)
            {
                selectedSlot.GetComponent<Image>().sprite = selectedSlot.initialimage;
                selectedSlot.transform.GetChild(1).GetComponent<Image>().sprite = null;
                selectedSlot.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
                selectedSlot.item = null;
            }
            else
            {
                selectedSlot.RemoveItem(1);
            }
            AudioController.controller.playaudio(1, 1);
            GameController.controller.uiController.RefreshUI();
            GameController.controller.uiController.SyncShopInventory();
        }
    }

    /// <summary>
    /// Remove an item from your inventory
    /// </summary>
    /// <param name="itemToRemove"></param>
    /// <param name="amount"></param>
    public void RemoveItem(Item itemToRemove, int amount) //aparentemente ninguem tá usando esse Remove do Inventário. Já chama o remove direto do slot.
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot currentSlot = transform.GetChild(i).GetComponent<Slot>();
            if (currentSlot.item == itemToRemove)
            {
                currentSlot.RemoveItem(amount);
            }
        }
    }

    public void AddGold(int amount)
    {
        goldAmount += amount;
        GameController.controller.uiController.goldtext.text = goldAmount.ToString();
    }

    public bool HasItem(Item theItem, int amount)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Slot>().item != null)
            {
                if (transform.GetChild(i).GetComponent<Slot>().item.itemName == theItem.itemName && transform.GetChild(i).GetComponent<Slot>().itemAmount >= amount)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckSpecificMat(string mat, int quantity)
    {
        int fullstack = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot currentslot = transform.GetChild(i).GetComponent<Slot>();
            if (currentslot.item != null && currentslot.item is CraftMat && currentslot.item.itemName == mat)
            {
                fullstack += currentslot.itemAmount;
                if (fullstack >= quantity)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveSpecificMat(string mat, int quantity)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Slot currentslot = transform.GetChild(i).GetComponent<Slot>();
            if (currentslot.item != null && currentslot.item is CraftMat && currentslot.item.itemName == mat)
            {
                if (quantity < currentslot.itemAmount)
                {
                    currentslot.RemoveItem(quantity);
                    return;
                }
                else
                {
                    quantity -= currentslot.itemAmount;
                    currentslot.RemoveItem(currentslot.itemAmount);
                }
            }
        }
    }

    public PlayerInfo SincronizeInv(PlayerInfo player1)
    {
        //Sync Equipped
        //Weapon Equipped
        if (weaponslot.equip != null)
        {
            player1.weaponprice = weaponslot.equip.price;
            player1.weapontype = weaponslot.equip.type;
            player1.weaponsubtype = weaponslot.equip.subType;
            player1.weaponmain = weaponslot.equip.mainAtribute;
            player1.weaponprimary = weaponslot.equip.primaryAtribute;
            player1.weaponsecondary1 = weaponslot.equip.secondaryAtribute1;
            player1.weaponsecondary2 = weaponslot.equip.secondaryAtribute2;
            player1.weapontier = weaponslot.equip.tier;
            player1.weaponmainvalue = weaponslot.equip.mainValue;
            player1.weaponprimaryvalue = weaponslot.equip.primaryValue;
            player1.weaponsecondary1value = weaponslot.equip.secondaryValue1;
            player1.weaponsecondary2value = weaponslot.equip.secondayValue2;
            switch (weaponslot.equip.subType)
            {
                case "Cardboard Sword":
                    player1.weaponsprite = 0;
                    break;
                case "Wooden Sword":
                    player1.weaponsprite = 1;
                    break;
                case "Iron Sword":
                    player1.weaponsprite = 2;
                    break;
            }
        }
        else
        {
            player1.weaponprice = 0;
            player1.weapontype = null;
            player1.weaponsubtype = null;
            player1.weaponmain = null;
            player1.weaponprimary = null;
            player1.weaponsecondary1 = null;
            player1.weaponsecondary2 = null;
            player1.weapontier = 0;
            player1.weaponmainvalue = 0;
            player1.weaponprimaryvalue = 0;
            player1.weaponsecondary1value = 0;
            player1.weaponsecondary2value = 0;
            player1.weaponsprite = 0;
        }

        //Shield Equipped
        if (shieldslot.equip != null)
        {
            player1.shieldprice = shieldslot.equip.price;
            player1.shieldtype = shieldslot.equip.type;
            player1.shieldsubtype = shieldslot.equip.subType;
            player1.shieldmain = shieldslot.equip.mainAtribute;
            player1.shieldprimary = shieldslot.equip.primaryAtribute;
            player1.shieldsecondary1 = shieldslot.equip.secondaryAtribute1;
            player1.shieldsecondary2 = shieldslot.equip.secondaryAtribute2;
            player1.shieldtier = shieldslot.equip.tier;
            player1.shieldmainvalue = shieldslot.equip.mainValue;
            player1.shieldprimaryvalue = shieldslot.equip.primaryValue;
            player1.shieldsecondary1value = shieldslot.equip.secondaryValue1;
            player1.shieldsecondary2value = shieldslot.equip.secondayValue2;
            switch (shieldslot.equip.subType)
            {
                case "Cardboard Shield":
                    player1.shieldsprite = 3;
                    break;
                case "Wooden Shield":
                    player1.shieldsprite = 4;
                    break;
                case "Iron Shield":
                    player1.shieldsprite = 5;
                    break;
            }
        }
        else
        {
            player1.shieldprice = 0;
            player1.shieldtype = null;
            player1.shieldsubtype = null;
            player1.shieldmain = null;
            player1.shieldprimary = null;
            player1.shieldsecondary1 = null;
            player1.shieldsecondary2 = null;
            player1.shieldtier = 0;
            player1.shieldmainvalue = 0;
            player1.shieldprimaryvalue = 0;
            player1.shieldsecondary1value = 0;
            player1.shieldsecondary2value = 0;
            player1.shieldsprite = 0;
        }

        //Head Equipped
        if (headslot.equip != null)
        {
            player1.headprice = headslot.equip.price;
            player1.headtype = headslot.equip.type;
            player1.headsubtype = headslot.equip.subType;
            player1.headmain = headslot.equip.mainAtribute;
            player1.headprimary = headslot.equip.primaryAtribute;
            player1.headsecondary1 = headslot.equip.secondaryAtribute1;
            player1.headsecondary2 = headslot.equip.secondaryAtribute2;
            player1.headtier = headslot.equip.tier;
            player1.headmainvalue = headslot.equip.mainValue;
            player1.headprimaryvalue = headslot.equip.primaryValue;
            player1.headsecondary1value = headslot.equip.secondaryValue1;
            player1.headsecondary2value = headslot.equip.secondayValue2;
            switch (headslot.equip.subType)
            {
                case "Cardboard Helmet":
                    player1.headsprite = 6;
                    break;
                case "Wooden Helmet":
                    player1.headsprite = 7;
                    break;
                case "Iron Helmet":
                    player1.headsprite = 8;
                    break;
            }
        }
        else
        {
            player1.headprice = 0;
            player1.headtype = null;
            player1.headsubtype = null;
            player1.headmain = null;
            player1.headprimary = null;
            player1.headsecondary1 = null;
            player1.headsecondary2 = null;
            player1.headtier = 0;
            player1.headmainvalue = 0;
            player1.headprimaryvalue = 0;
            player1.headsecondary1value = 0;
            player1.headsecondary2value = 0;
            player1.headsprite = 0;
        }

        //Body Equipped
        if (bodyslot.equip != null)
        {
            player1.bodyprice = bodyslot.equip.price;
            player1.bodytype = bodyslot.equip.type;
            player1.bodysubtype = bodyslot.equip.subType;
            player1.bodymain = bodyslot.equip.mainAtribute;
            player1.bodyprimary = bodyslot.equip.primaryAtribute;
            player1.bodysecondary1 = bodyslot.equip.secondaryAtribute1;
            player1.bodysecondary2 = bodyslot.equip.secondaryAtribute2;
            player1.bodytier = bodyslot.equip.tier;
            player1.bodymainvalue = bodyslot.equip.mainValue;
            player1.bodyprimaryvalue = bodyslot.equip.primaryValue;
            player1.bodysecondary1value = bodyslot.equip.secondaryValue1;
            player1.bodysecondary2value = bodyslot.equip.secondayValue2;
            switch (bodyslot.equip.subType)
            {
                case "Cardboard Armor":
                    player1.bodysprite = 9;
                    break;
                case "Wooden Armor":
                    player1.bodysprite = 10;
                    break;
                case "Iron Armor":
                    player1.bodysprite = 11;
                    break;
            }
        }
        else
        {
            player1.bodyprice = 0;
            player1.bodytype = null;
            player1.bodysubtype = null;
            player1.bodymain = null;
            player1.bodyprimary = null;
            player1.bodysecondary1 = null;
            player1.bodysecondary2 = null;
            player1.bodytier = 0;
            player1.bodymainvalue = 0;
            player1.bodyprimaryvalue = 0;
            player1.bodysecondary1value = 0;
            player1.bodysecondary2value = 0;
            player1.bodysprite = 0;
        }

        //Foot Equipped
        if (footslot.equip != null)
        {
            player1.footprice = footslot.equip.price;
            player1.foottype = footslot.equip.type;
            player1.footsubtype = footslot.equip.subType;
            player1.footmain = footslot.equip.mainAtribute;
            player1.footprimary = footslot.equip.primaryAtribute;
            player1.footsecondary1 = footslot.equip.secondaryAtribute1;
            player1.footsecondary2 = footslot.equip.secondaryAtribute2;
            player1.foottier = footslot.equip.tier;
            player1.footmainvalue = footslot.equip.mainValue;
            player1.footprimaryvalue = footslot.equip.primaryValue;
            player1.footsecondary1value = footslot.equip.secondaryValue1;
            player1.footsecondary2value = footslot.equip.secondayValue2;
            switch (footslot.equip.subType)
            {
                case "Cardboard Shoes":
                    player1.footsprite = 12;
                    break;
                case "Wooden Shoes":
                    player1.footsprite = 13;
                    break;
                case "Iron Shoes":
                    player1.footsprite = 14;
                    break;
            }
        }
        else
        {
            player1.footprice = 0;
            player1.foottype = null;
            player1.footsubtype = null;
            player1.footmain = null;
            player1.footprimary = null;
            player1.footsecondary1 = null;
            player1.footsecondary2 = null;
            player1.foottier = 0;
            player1.footmainvalue = 0;
            player1.footprimaryvalue = 0;
            player1.footsecondary1value = 0;
            player1.footsecondary2value = 0;
            player1.footsprite = 0;
        }

        //Sync Inventory Slots

        for (int i = 0; i < transform.childCount; i++)
        {
            //Zera tudo para não ficar com resíduos de saves anteriores
            player1.itemtype[i] = null;
            player1.itemsubtype[i] = null;
            player1.itemmain[i] = null;
            player1.itemprimary[i] = null;
            player1.itemsecondary1[i] = null;
            player1.itemsecondary2[i] = null;
            player1.itemtier[i] = 0;
            player1.itemmainvalue[i] = 0;
            player1.itemprimaryvalue[i] = 0;
            player1.itemsecondary1value[i] = 0;
            player1.itemsecondary2value[i] = 0;
            player1.itemsprite[i] = 0;
            player1.itemmaxstackamount[i] = 0;
            player1.itemprice[i] = 0;
            player1.itemname[i] = null;
            player1.itemamount[i] = 0;

            Slot currentslot = transform.GetChild(i).GetComponent<Slot>();
            if (currentslot.item != null)
            {
                if (currentslot.item is Equipable)
                {
                    Equipable equipaux = (Equipable)currentslot.item;

                    player1.itemprice[i] = equipaux.price;
                    player1.itemtype[i] = equipaux.type;
                    player1.itemsubtype[i] = equipaux.subType;
                    player1.itemmain[i] = equipaux.mainAtribute;
                    player1.itemprimary[i] = equipaux.primaryAtribute;
                    player1.itemsecondary1[i] = equipaux.secondaryAtribute1;
                    player1.itemsecondary2[i] = equipaux.secondaryAtribute2;
                    player1.itemtier[i] = equipaux.tier;
                    player1.itemmainvalue[i] = equipaux.mainValue;
                    player1.itemprimaryvalue[i] = equipaux.primaryValue;
                    player1.itemsecondary1value[i] = equipaux.secondaryValue1;
                    player1.itemsecondary2value[i] = equipaux.secondayValue2;
                    switch (equipaux.subType)
                    {
                        case "Cardboard Sword":
                            player1.itemsprite[i] = 0;
                            break;
                        case "Wooden Sword":
                            player1.itemsprite[i] = 1;
                            break;
                        case "Iron Sword":
                            player1.itemsprite[i] = 2;
                            break;
                        case "Cardboard Shield":
                            player1.itemsprite[i] = 3;
                            break;
                        case "Wooden Shield":
                            player1.itemsprite[i] = 4;
                            break;
                        case "Iron Shield":
                            player1.itemsprite[i] = 5;
                            break;
                        case "Cardboard Helmet":
                            player1.itemsprite[i] = 6;
                            break;
                        case "Wooden Helmet":
                            player1.itemsprite[i] = 7;
                            break;
                        case "Iron Helmet":
                            player1.itemsprite[i] = 8;
                            break;
                        case "Cardboard Armor":
                            player1.itemsprite[i] = 9;
                            break;
                        case "Wooden Armor":
                            player1.itemsprite[i] = 10;
                            break;
                        case "Iron Armor":
                            player1.itemsprite[i] = 11;
                            break;
                        case "Cardboard Shoes":
                            player1.itemsprite[i] = 12;
                            break;
                        case "Wooden Shoes":
                            player1.itemsprite[i] = 13;
                            break;
                        case "Iron Shoes":
                            player1.itemsprite[i] = 14;
                            break;
                    }
                }
                else if (currentslot.item is CraftMat)
                {
                    CraftMat equipaux = (CraftMat)currentslot.item;
                    player1.itemmaxstackamount[i] = equipaux.maxstackamount;
                    player1.itemprice[i] = equipaux.price;
                    player1.itemname[i] = equipaux.itemName;
                    player1.itemamount[i] = currentslot.itemAmount;
                    switch (equipaux.itemName)
                    {
                        case "Red":
                            player1.itemsprite[i] = 0;
                            break;
                        case "Blue":
                            player1.itemsprite[i] = 1;
                            break;
                        case "Yellow":
                            player1.itemsprite[i] = 2;
                            break;
                        case "Orange":
                            player1.itemsprite[i] = 3;
                            break;
                        case "Black":
                            player1.itemsprite[i] = 4;
                            break;
                    }
                }
            }
        }
        return player1;
    }

    public void LoadInventory(PlayerInfo player1)
    {
        //Clear EquipSlots
        weaponslot.equip = null;
        weaponslot.GetComponent<Image>().sprite = weaponslot.initialimage;
        weaponslot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        weaponslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        shieldslot.equip = null;
        shieldslot.GetComponent<Image>().sprite = shieldslot.initialimage;
        shieldslot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        shieldslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        headslot.equip = null;
        headslot.GetComponent<Image>().sprite = headslot.initialimage;
        headslot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        headslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        bodyslot.equip = null;
        bodyslot.GetComponent<Image>().sprite = bodyslot.initialimage;
        bodyslot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        bodyslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        footslot.equip = null;
        footslot.GetComponent<Image>().sprite = footslot.initialimage;
        footslot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        footslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);

        //Load Equips
        //Load Weapon
        if (player1.weapontype != null)
        {
            Equipable weaponequipped = new Equipable();

            weaponequipped.itemName = player1.weaponsubtype;
            weaponequipped.itemIcon = DataBase.dataBase.myEquipSprites[player1.weaponsprite];
            weaponequipped.price = player1.weaponprice;

            weaponequipped.type = player1.weapontype;
            weaponequipped.subType = player1.weaponsubtype;
            weaponequipped.mainAtribute = player1.weaponmain;
            weaponequipped.primaryAtribute = player1.weaponprimary;
            weaponequipped.secondaryAtribute1 = player1.weaponsecondary1;
            weaponequipped.secondaryAtribute2 = player1.weaponsecondary2;
            weaponequipped.tier = player1.weapontier;
            weaponequipped.mainValue = player1.weaponmainvalue;
            weaponequipped.primaryValue = player1.weaponprimaryvalue;
            weaponequipped.secondaryValue1 = player1.weaponsecondary1value;
            weaponequipped.secondayValue2 = player1.weaponsecondary2value;

            weaponslot.equip = weaponequipped;
            weaponslot.GetComponent<Image>().sprite = DataBase.dataBase.myEquipSprites[player1.weaponsprite];
            weaponslot.transform.GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[player1.weapontier - 1];
            weaponslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);

            weaponslot.sword.SetActive(true);
        }

        //Load Shield
        if (player1.shieldtype != null)
        {
            Equipable shieldequipped = new Equipable();

            shieldequipped.itemName = player1.shieldsubtype;
            shieldequipped.itemIcon = DataBase.dataBase.myEquipSprites[player1.shieldsprite];
            shieldequipped.price = player1.shieldprice;

            shieldequipped.type = player1.shieldtype;
            shieldequipped.subType = player1.shieldsubtype;
            shieldequipped.mainAtribute = player1.shieldmain;
            shieldequipped.primaryAtribute = player1.shieldprimary;
            shieldequipped.secondaryAtribute1 = player1.shieldsecondary1;
            shieldequipped.secondaryAtribute2 = player1.shieldsecondary2;
            shieldequipped.tier = player1.shieldtier;
            shieldequipped.mainValue = player1.shieldmainvalue;
            shieldequipped.primaryValue = player1.shieldprimaryvalue;
            shieldequipped.secondaryValue1 = player1.shieldsecondary1value;
            shieldequipped.secondayValue2 = player1.shieldsecondary2value;


            shieldslot.equip = shieldequipped;
            shieldslot.GetComponent<Image>().sprite = DataBase.dataBase.myEquipSprites[player1.shieldsprite];
            shieldslot.transform.GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[player1.shieldtier - 1];
            shieldslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            shieldslot.shield.SetActive(true);
        }

        //Load Head
        if (player1.headtype != null)
        {
            Equipable headequipped = new Equipable();

            headequipped.itemName = player1.headsubtype;
            headequipped.itemIcon = DataBase.dataBase.myEquipSprites[player1.headsprite];
            headequipped.price = player1.headprice;

            headequipped.type = player1.headtype;
            headequipped.subType = player1.headsubtype;
            headequipped.mainAtribute = player1.headmain;
            headequipped.primaryAtribute = player1.headprimary;
            headequipped.secondaryAtribute1 = player1.headsecondary1;
            headequipped.secondaryAtribute2 = player1.headsecondary2;
            headequipped.tier = player1.headtier;
            headequipped.mainValue = player1.headmainvalue;
            headequipped.primaryValue = player1.headprimaryvalue;
            headequipped.secondaryValue1 = player1.headsecondary1value;
            headequipped.secondayValue2 = player1.headsecondary2value;

            headslot.equip = headequipped;
            headslot.GetComponent<Image>().sprite = DataBase.dataBase.myEquipSprites[player1.headsprite];
            headslot.transform.GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[player1.headtier - 1];
            headslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        }

        //Load Body
        if (player1.bodytype != null)
        {
            Equipable bodyequipped = new Equipable();

            bodyequipped.itemName = player1.bodysubtype;
            bodyequipped.itemIcon = DataBase.dataBase.myEquipSprites[player1.bodysprite];
            bodyequipped.price = player1.bodyprice;

            bodyequipped.type = player1.bodytype;
            bodyequipped.subType = player1.bodysubtype;
            bodyequipped.mainAtribute = player1.bodymain;
            bodyequipped.primaryAtribute = player1.bodyprimary;
            bodyequipped.secondaryAtribute1 = player1.bodysecondary1;
            bodyequipped.secondaryAtribute2 = player1.bodysecondary2;
            bodyequipped.tier = player1.bodytier;
            bodyequipped.mainValue = player1.bodymainvalue;
            bodyequipped.primaryValue = player1.bodyprimaryvalue;
            bodyequipped.secondaryValue1 = player1.bodysecondary1value;
            bodyequipped.secondayValue2 = player1.bodysecondary2value;

            bodyslot.equip = bodyequipped;
            bodyslot.GetComponent<Image>().sprite = DataBase.dataBase.myEquipSprites[player1.bodysprite];
            bodyslot.transform.GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[player1.bodytier - 1];
            bodyslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        }

        //Load Foot
        if (player1.foottype != null)
        {
            Equipable footequipped = new Equipable();

            footequipped.itemName = player1.footsubtype;
            footequipped.itemIcon = DataBase.dataBase.myEquipSprites[player1.footsprite];
            footequipped.price = player1.footprice;

            footequipped.type = player1.foottype;
            footequipped.subType = player1.footsubtype;
            footequipped.mainAtribute = player1.footmain;
            footequipped.primaryAtribute = player1.footprimary;
            footequipped.secondaryAtribute1 = player1.footsecondary1;
            footequipped.secondaryAtribute2 = player1.footsecondary2;
            footequipped.tier = player1.foottier;
            footequipped.mainValue = player1.footmainvalue;
            footequipped.primaryValue = player1.footprimaryvalue;
            footequipped.secondaryValue1 = player1.footsecondary1value;
            footequipped.secondayValue2 = player1.footsecondary2value;

            footslot.equip = footequipped;
            footslot.GetComponent<Image>().sprite = DataBase.dataBase.myEquipSprites[player1.footsprite];
            footslot.transform.GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[player1.foottier - 1];
            footslot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        }
        //Load Inventory
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot currentslot = transform.GetChild(i).GetComponent<Slot>();
            currentslot.RemoveItem(currentslot.itemAmount);
            //Se tiver um equipable salvo na posição
            if (player1.itemtype[i] != null)
            {
                Equipable itemequipped = new Equipable();

                itemequipped.itemName = player1.itemsubtype[i];
                itemequipped.itemIcon = DataBase.dataBase.myEquipSprites[player1.itemsprite[i]];
                itemequipped.price = player1.itemprice[i];

                itemequipped.type = player1.itemtype[i];
                itemequipped.subType = player1.itemsubtype[i];
                itemequipped.mainAtribute = player1.itemmain[i];
                itemequipped.primaryAtribute = player1.itemprimary[i];
                itemequipped.secondaryAtribute1 = player1.itemsecondary1[i];
                itemequipped.secondaryAtribute2 = player1.itemsecondary2[i];
                itemequipped.tier = player1.itemtier[i];
                itemequipped.mainValue = player1.itemmainvalue[i];
                itemequipped.primaryValue = player1.itemprimaryvalue[i];
                itemequipped.secondaryValue1 = player1.itemsecondary1value[i];
                itemequipped.secondayValue2 = player1.itemsecondary2value[i];

                currentslot.AddItem(itemequipped, 1);
            }
            //Se tiver um material salvo na posição
            else if (player1.itemname[i] != null)
            {
                CraftMat matequipped = new CraftMat();

                matequipped.itemName = player1.itemname[i];
                matequipped.itemIcon = DataBase.dataBase.myMaterialSprites[player1.itemsprite[i]];
                matequipped.price = player1.itemprice[i];
                matequipped.maxstackamount = player1.itemmaxstackamount[i];

                currentslot.itemAmount = 0;
                currentslot.AddItem(matequipped, player1.itemamount[i]);
            }
        }
    }
}
