using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Recebe no Start
    private Shop shopScript;
    private Image icon;
    public Image itemBorder;
    public int quantity;
    public Text quantitytext;
    public GameObject selectedborder;

    //Recebe esse item do método GenSlots ou GenMaterials no Awake da Loja (antes de tudo quando entra em uma loja)
    public Item item;


    private void Start()
    {
        RefreshShopSlot();
    }

    public void OnPointerEnter(PointerEventData eventdata)
    {
        if (item != null)
        {
            if (item is Equipable)
            {
                Equipable myitem = (Equipable)item;
                GameController.controller.uiController.ShowEquipableTooltip(true, myitem);
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
        GameController.controller.uiController.ShowEquipableTooltip(false, null);
        GameController.controller.uiController.ShowCraftMatTooltip(false, null);
    }

    public void RefreshShopSlot()
    {
        if (item == null)
        {
            Destroy(gameObject);
        }
        else
        {
            shopScript = GameObject.FindObjectOfType<Shop>(); // Encontra a loja que tiver ativa
            icon = GetComponent<Image>(); // referencia para icone
            icon.sprite = item.itemIcon; //altera o icone do slot da loja
            if (shopScript.gameObject.tag=="Blacksmith")
            {
                Equipable myitem = (Equipable)item;
                itemBorder.sprite = DataBase.dataBase.myTierSprites[myitem.tier - 1];
                itemBorder.color = new Color(1, 1, 1, 0.3f);
                transform.GetChild(2).GetComponent<Text>().text = item.price.ToString(); // altera o preço do slot da loja
            }
            else
            {
                CraftMat myitem = (CraftMat)item;
                transform.GetChild(1).GetComponent<Text>().text = item.price.ToString(); // altera o preço do slot da loja
                quantity = Random.Range(1, 6); 
                quantitytext.text = quantity.ToString(); // altera a quantidade do slot da loja
            }
        }
    }

    public void SelectSlot()
    {
        transform.parent.GetComponent<ShopSlotGrid>().ResetSelected();
        shopScript.SelectShopSlot(this);
        selectedborder.SetActive(true);
    }
}
