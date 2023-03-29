using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Inventory inventory;
    public GameObject shopPanel;
    public ShopSlot shopSlotPrefab;
    private ShopSlot SelectedShopSlot;
    private Player player;


    void Awake()
    {
        player = GameController.controller.player.GetComponent<Player>();
        if (this.gameObject.tag == "Blacksmith")
        {
            GenItens(); // Gera os itens da loja
        }
        else
        {
            GenMaterials();// Gera os itens da loja
        }

    }

    /// <summary>
    /// Generates Items and put them in Shop Slots
    /// </summary>
    public void GenItens()
    {
        CreateShopSlots();
        for (int i = 0; i < shopPanel.transform.childCount; i++) // Roda cada slot da loja
        {
            //Trata dos Slots que foram comprados
            shopPanel.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1); //Deixa claro novamente
            shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().itemBorder.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            shopPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(true); // Reativa o fundo do preço
            shopPanel.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true); // Reativa o preço
            shopPanel.transform.GetChild(i).GetComponent<Button>().interactable = true; // Torna o Slot clicável 

            if (shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().item != null) // se já tiver item no slot
            {
                shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().item = null; // elimina o item do slot
            }
            if (Random.Range(0, 100) < 50) // chance de 50% de criar um item para esse slot
            {
                Equipable myitem = new Equipable();
                myitem = DataBase.dataBase.CreateEquipableByTier(1); // Gera um item aleatório de tier 1

                //Bargain Passive
                if (player.passivesManager.getBargainUnlocked())
                {
                    myitem.price -= (int)(myitem.price * (player.passivesManager.getBargainPercentage() / 100f));
                }
                shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().item = myitem; // coloca o item gerado dentro do atributo Item desse slot
            }
        }
    }

    public void GenMaterials()
    {
        CreateShopSlots();
        for (int i = 0; i < shopPanel.transform.childCount; i++) // Roda cada slot da loja
        {
            //Trata dos Slots que foram comprados (REATIVA OS 3 FILHOS do slot em questão)
            shopPanel.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1); //Deixa claro novamente
            shopPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            shopPanel.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
            shopPanel.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
            shopPanel.transform.GetChild(i).GetComponent<Button>().interactable = true; // Torna o Slot clicável 

            if (shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().item != null) // se já tiver item no slot
            {
                shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().item = null; // elimina o item do slot
            }

            CraftMat myitem = new CraftMat();
            switch (i)
            {
                case 0:
                    //Cardboard
                    if (Random.Range(0, 100) < 70)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Cardboard");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 1:
                    //Wood
                    if (Random.Range(0, 100) < 30)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Wood");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 2:
                    //Iron
                    if (Random.Range(0, 100) < 5)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Iron");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 3:
                    //Goo
                    if (Random.Range(0, 100) < 70)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Goo");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 4:
                    //Honey
                    if (Random.Range(0, 100) < 70)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Honey");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 5:
                    //Vine
                    if (Random.Range(0, 100) < 30)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Vine");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 6:
                    //Bones
                    if (Random.Range(0, 100) < 30)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Bones");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 7:
                    //Cactus Flower
                    if (Random.Range(0, 100) < 30)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Cactus Flower");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 8:
                    //Leather
                    if (Random.Range(0, 100) < 5)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Leather");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                case 9:
                    //Stones
                    if (Random.Range(0, 100) < 5)
                    {
                        myitem = DataBase.dataBase.CreateSpecificMaterial("Stones");
                    }
                    else
                    {
                        myitem = null;
                    }
                    break;
                default:
                    break;
            }
            //Bargain Passive
            if (myitem != null && player.passivesManager.getBargainUnlocked())
            {
                myitem.price -= (int)(myitem.price * (player.passivesManager.getBargainPercentage() / 100f));
            }
            shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().item = myitem; // coloca o item gerado dentro do atributo Item desse slot
        }
    }


    /// <summary>
    /// Resets Shops Slots back to 18
    /// </summary>
    public void CreateShopSlots()
    {
        if (this.gameObject.tag == "Blacksmith")
        {
            while (shopPanel.transform.childCount < 18)
            {
                Instantiate(shopSlotPrefab, shopPanel.transform);
            }
        }
        else
        {
            while (shopPanel.transform.childCount < 10)
            {
                Instantiate(shopSlotPrefab, shopPanel.transform);
            }
        }

    }

    /// <summary>
    /// Método chamado in-game pelo botão de compra da loja
    /// </summary>
    public void BuyItem()
    {
        if (SelectedShopSlot != null && SelectedShopSlot.item != null) //o botão só funciona se tiver selecionado um slot e se o slot não estiver vazio
        {
            if (inventory.goldAmount >= SelectedShopSlot.item.price)
            {
                bool bought = inventory.AddItem(SelectedShopSlot.item, 1);

                if (bought)
                {
                    inventory.goldAmount -= SelectedShopSlot.item.price;
                    if (this.gameObject.tag == "Blacksmith")
                    {
                        //SelectedSlot.item.itemIcon = um novo icone padrao de item comprado;
                        SelectedShopSlot.itemBorder.color = new Color(1, 1, 1, 0); //Deixa o negocio negoçado
                        SelectedShopSlot.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f); // escurece o item na loja
                        //SelectedShopSlot.transform.GetChild(1).gameObject.SetActive(false); // esconde o fundo do preço
                        //SelectedShopSlot.transform.GetChild(2).gameObject.SetActive(false); // esconde o preço do item na loja
                        SelectedShopSlot.transform.GetChild(2).GetComponent<Text>().text = "SOLD!"; // escreve SOLD no item
                        SelectedShopSlot.transform.GetChild(3).gameObject.SetActive(false); // esconde a bordinha
                        SelectedShopSlot.GetComponent<Button>().interactable = false; // não da pra clicar no item mais
                        DeselectShopSlot();// tira a seleção do slot que foi comprado (para evita comprar novamente)
                    }
                    else
                    {
                        SelectedShopSlot.quantity--;
                        SelectedShopSlot.quantitytext.text = SelectedShopSlot.quantity.ToString(); // altera a quantidade do slot da loja
                        if (SelectedShopSlot.quantity <= 0)
                        {
                            //Acabou os itens
                            //SelectedSlot.item.itemIcon = um novo icone padrao de item comprado;
                            SelectedShopSlot.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f); // escurece o item na loja
                            //SelectedShopSlot.transform.GetChild(0).gameObject.SetActive(false); // esconde o fundo do preço
                            //SelectedShopSlot.transform.GetChild(1).gameObject.SetActive(false); // esconde o preço do item na loja
                            SelectedShopSlot.transform.GetChild(1).GetComponent<Text>().text = "SOLD!"; // escreve SOLD no item
                            SelectedShopSlot.transform.GetChild(2).gameObject.SetActive(false); // esconde a quantidade
                            SelectedShopSlot.transform.GetChild(3).gameObject.SetActive(false); // esconde a bordinha
                            SelectedShopSlot.GetComponent<Button>().interactable = false; // não da pra clicar no item mais
                            DeselectShopSlot();// tira a seleção do slot que foi comprado (para evita comprar novamente)
                        }
                    }
                    AudioController.controller.playaudio(0,1);
                    GameController.controller.uiController.RefreshUI(); // Atualiza a UI (inventário no caso) para atualizar as cópias de inventário na loja
                    GameController.controller.uiController.SyncShopInventory(); // Atualiza a UI de todos os elementos-cópia do inventório dentro da loja ativa
                }
            }
            else
            {
                Debug.Log("You don't have enough money.");
            }
        }
    }

    public void SelectShopSlot(ShopSlot shopslot)
    {
        SelectedShopSlot = shopslot;
    }

    public void DeselectShopSlot()
    {
        SelectedShopSlot = null;
    }
}