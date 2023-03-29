using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftGrid : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image recipeMirror;
    [SerializeField] private GameObject mirrorMaterialGrid;
    [SerializeField] private GameObject mirrorNumMaterialGrid;
    [SerializeField] private Text itemNameDescription;
    [SerializeField] private Text atributeMainDescription;
    [SerializeField] private Text atributePrimaryDescription;
    [SerializeField] private Text atributeSecondary1Description;
    [SerializeField] private Text atributeSecondary2Description;
    [SerializeField] private GameObject coinCraftPrice;

    public RecipeSlot SelectedRecipeSlot;
    private bool material1ok = true;
    private bool material2ok = true;
    private bool material3ok = true;
    private bool material4ok = true;

    private void Start()
    {
        ClearMaterialMirrors();
    }

    public void SelectRecipeSlot(RecipeSlot recipeslot)
    {
        if (recipeslot != null)
        {
            ResetHasMaterials();
            ClearMaterialMirrors();
            SelectedRecipeSlot = recipeslot;
            recipeMirror.sprite = recipeslot.recipe.icon;
            recipeMirror.GetComponent<Image>().color = Color.white;
            itemNameDescription.gameObject.SetActive(true);
            if (SelectedRecipeSlot.recipe.type == "Potion")
            {
                itemNameDescription.text = SelectedRecipeSlot.recipe.type;
                atributeMainDescription.gameObject.SetActive(true);
                atributeMainDescription.text = "Heals " + GameController.controller.player.GetComponent<Player>().potioneffect + " %\nof your max life";
                mirrorMaterialGrid.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myMaterialSprites[ChooseMaterialSprite(SelectedRecipeSlot.recipe.material1)];
                mirrorMaterialGrid.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
                mirrorNumMaterialGrid.transform.GetChild(0).GetComponent<Text>().text = Matcount(SelectedRecipeSlot.recipe.material1) + " / " + SelectedRecipeSlot.recipe.material1quantity.ToString();
                mirrorNumMaterialGrid.transform.GetChild(0).gameObject.SetActive(true);
                if (!inventory.CheckSpecificMat(SelectedRecipeSlot.recipe.material1, SelectedRecipeSlot.recipe.material1quantity))
                {
                    mirrorMaterialGrid.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    material1ok = false;
                }
                else
                {
                    mirrorMaterialGrid.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    material1ok = true;
                }
            }
            else
            {
                if (SelectedRecipeSlot.recipe.subtype == "")
                {
                    itemNameDescription.text = "Random " + SelectedRecipeSlot.recipe.type;
                }
                else
                {
                    itemNameDescription.text = "Random " + SelectedRecipeSlot.recipe.subtype;
                }
                atributeMainDescription.gameObject.SetActive(true);
                atributeMainDescription.text = "+ ?? " + SelectMainAtribute();
                atributePrimaryDescription.gameObject.SetActive(true);
                atributePrimaryDescription.text = "+ ?? Random Primary";
                coinCraftPrice.SetActive(true);
                coinCraftPrice.transform.GetChild(0).GetComponent<Text>().text = SelectedRecipeSlot.recipe.price.ToString();

                if (SelectedRecipeSlot.recipe.material1 != "")
                {
                    mirrorMaterialGrid.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myMaterialSprites[ChooseMaterialSprite(SelectedRecipeSlot.recipe.material1)];
                    mirrorMaterialGrid.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
                    mirrorNumMaterialGrid.transform.GetChild(0).GetComponent<Text>().text = Matcount(SelectedRecipeSlot.recipe.material1) + " / " + SelectedRecipeSlot.recipe.material1quantity.ToString();
                    mirrorNumMaterialGrid.transform.GetChild(0).gameObject.SetActive(true);
                    if (!inventory.CheckSpecificMat(SelectedRecipeSlot.recipe.material1, SelectedRecipeSlot.recipe.material1quantity))
                    {
                        mirrorMaterialGrid.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 0, 1);
                        material1ok = false;
                    }
                    else
                    {
                        mirrorMaterialGrid.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                        material1ok = true;
                    }
                }
                if (SelectedRecipeSlot.recipe.material2 != "")
                {
                    mirrorMaterialGrid.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myMaterialSprites[ChooseMaterialSprite(SelectedRecipeSlot.recipe.material2)];
                    mirrorMaterialGrid.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.white;
                    mirrorNumMaterialGrid.transform.GetChild(1).GetComponent<Text>().text = Matcount(SelectedRecipeSlot.recipe.material2) + " / " + SelectedRecipeSlot.recipe.material2quantity.ToString();
                    mirrorNumMaterialGrid.transform.GetChild(1).gameObject.SetActive(true);
                    if (!inventory.CheckSpecificMat(SelectedRecipeSlot.recipe.material2, SelectedRecipeSlot.recipe.material2quantity))
                    {
                        mirrorMaterialGrid.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 0, 1);
                        material2ok = false;
                    }
                    else
                    {
                        mirrorMaterialGrid.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                        material2ok = true;
                    }
                }
                if (SelectedRecipeSlot.recipe.material3 != "")
                {
                    mirrorMaterialGrid.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myMaterialSprites[ChooseMaterialSprite(SelectedRecipeSlot.recipe.material3)];
                    mirrorMaterialGrid.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.white;
                    mirrorNumMaterialGrid.transform.GetChild(2).GetComponent<Text>().text = Matcount(SelectedRecipeSlot.recipe.material3) + " / " + SelectedRecipeSlot.recipe.material3quantity.ToString();
                    mirrorNumMaterialGrid.transform.GetChild(2).gameObject.SetActive(true);
                    if (!inventory.CheckSpecificMat(SelectedRecipeSlot.recipe.material3, SelectedRecipeSlot.recipe.material3quantity))
                    {
                        mirrorMaterialGrid.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 0, 0, 1);
                        material3ok = false;
                    }
                    else
                    {
                        mirrorMaterialGrid.transform.GetChild(2).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                        material3ok = true;
                    }
                }
                if (SelectedRecipeSlot.recipe.material4 != "")
                {
                    mirrorMaterialGrid.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myMaterialSprites[ChooseMaterialSprite(SelectedRecipeSlot.recipe.material4)];
                    mirrorMaterialGrid.transform.GetChild(3).GetChild(0).GetComponent<Image>().color = Color.white;
                    mirrorNumMaterialGrid.transform.GetChild(3).GetComponent<Text>().text = Matcount(SelectedRecipeSlot.recipe.material4) + " / " + SelectedRecipeSlot.recipe.material4quantity.ToString();
                    mirrorNumMaterialGrid.transform.GetChild(3).gameObject.SetActive(true);
                    if (!inventory.CheckSpecificMat(SelectedRecipeSlot.recipe.material4, SelectedRecipeSlot.recipe.material4quantity))
                    {
                        mirrorMaterialGrid.transform.GetChild(3).GetComponent<Image>().color = new Color(1, 0, 0, 1);
                        material4ok = false;
                    }
                    else
                    {
                        mirrorMaterialGrid.transform.GetChild(3).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                        material4ok = true;
                    }
                }

                switch (SelectedRecipeSlot.recipe.tier)
                {
                    case 2:
                        atributeSecondary1Description.text = "+ ?? Random Secondary";
                        atributeSecondary1Description.gameObject.SetActive(true);
                        break;

                    case 3:
                        atributeSecondary1Description.text = "+ ?? Random Secondary";
                        atributeSecondary1Description.gameObject.SetActive(true);
                        atributeSecondary2Description.text = "+ ?? Random Secondary";
                        atributeSecondary2Description.gameObject.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void ResetHasMaterials()
    {
        material1ok = true;
        material2ok = true;
        material3ok = true;
        material4ok = true;
    }

    private string SelectMainAtribute()
    {
        switch (SelectedRecipeSlot.recipe.type)
        {
            case "Weapon":
                return "Melee Damage";
            default:
                return "Armor";
        }
    }

    private bool HasMAllMaterials()
    {
        if (material1ok && material2ok && material3ok && material4ok)
        {
            return true;
        }
        return false;
    }

    public void ClearMaterialMirrors()
    {
        recipeMirror.sprite = null;
        recipeMirror.GetComponent<Image>().color = new Color(0.5490196f, 0.4980392f, 0.4078432f);
        itemNameDescription.gameObject.SetActive(false);
        atributeMainDescription.gameObject.SetActive(false);
        atributePrimaryDescription.gameObject.SetActive(false);
        atributeSecondary1Description.gameObject.SetActive(false);
        atributeSecondary2Description.gameObject.SetActive(false);
        coinCraftPrice.SetActive(false);

        for (int i = 0; i < mirrorMaterialGrid.transform.childCount; i++)
        {
            mirrorMaterialGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            mirrorMaterialGrid.transform.GetChild(i).GetComponent<Image>().color = new Color(0, 0, 0, 0);
            mirrorMaterialGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(0.5490196f, 0.4980392f, 0.4078432f);
        }
        for (int i = 0; i < mirrorNumMaterialGrid.transform.childCount; i++)
        {
            mirrorNumMaterialGrid.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public int ChooseMaterialSprite(string materialname)
    {
        switch (materialname)
        {
            case "Cardboard": return 0;
            case "Wood": return 1;
            case "Iron": return 2;
            case "Goo": return 3;
            case "Honey": return 4;
            case "Vine": return 5;
            case "Bones": return 6;
            case "Cactus Flower": return 7;
            case "Leather": return 8;
            case "Stones": return 9;
            default: return 0;
        }
    }

    public void CraftItem()
    {
        if (SelectedRecipeSlot != null) //o botão só funciona se tiver selecionado um slot
        {
            if (SelectedRecipeSlot.recipe.type == "Potion")
            {
                if (inventory.goldAmount >= SelectedRecipeSlot.recipe.price && HasMAllMaterials())
                {
                    inventory.goldAmount -= SelectedRecipeSlot.recipe.price; // subtrai o dinheiro
                    RemoveInventoryMat(); // remove os materiais
                    GameController.controller.uiController.potionref.potionquantity++;
                    SelectRecipeSlot(SelectedRecipeSlot);
                    GameController.controller.uiController.RefreshInventory();
                    GameController.controller.uiController.RefreshUI();
                }
            }
            else
            {
                //Verifica se tem Gold e os materiais necessários
                if (inventory.goldAmount >= SelectedRecipeSlot.recipe.price && HasMAllMaterials())
                {
                    //cria o item dentro do inventário
                    Equipable itemtocraft = DataBase.dataBase.CreateSpecificEquipable(SelectedRecipeSlot.recipe.tier, SelectedRecipeSlot.recipe.type, SelectedRecipeSlot.recipe.subtype);
                    bool crafted = inventory.AddItem(itemtocraft, 1);
                    //Verifica se conseguiu criar (inventário tem espaço)
                    if (crafted)
                    {

                        inventory.goldAmount -= SelectedRecipeSlot.recipe.price; // subtrai o dinheiro
                        RemoveInventoryMat(); // remove os materiais

                        //Altera Descrição
                        itemNameDescription.text = itemtocraft.subType;
                        atributeMainDescription.text = "+ " + itemtocraft.mainValue.ToString() + " " + SelectMainAtribute();
                        atributePrimaryDescription.text = "+ " + itemtocraft.primaryValue.ToString() + " " + itemtocraft.primaryAtribute;
                        coinCraftPrice.SetActive(false);

                        switch (SelectedRecipeSlot.recipe.tier)
                        {
                            case 2:
                                atributeSecondary1Description.text = "+ " + itemtocraft.secondaryValue1.ToString() + " " + itemtocraft.secondaryAtribute1;
                                break;
                            case 3:
                                atributeSecondary1Description.text = "+ " + itemtocraft.secondaryValue1.ToString() + " " + itemtocraft.secondaryAtribute1;
                                atributeSecondary2Description.text = "+ " + itemtocraft.secondayValue2.ToString() + " " + itemtocraft.secondaryAtribute2;
                                break;
                            default:
                                break;
                        }
                        SelectRecipeSlot(SelectedRecipeSlot);
                        GameController.controller.uiController.RefreshInventory();
                        GameController.controller.uiController.RefreshUI(); // Atualiza a UI
                        GameController.controller.uiController.SyncShopInventory(); // Atualiza a UI de todos os elementos-cópia do inventório dentro da loja ativa
                    }
                    else
                    {
                        Debug.Log("Impossible to craft. Inventory is Full!");
                    }
                }
                else
                {
                    Debug.Log("You don't have enough money or materials.");
                }
            }
        }
        SelectRecipeSlot(null);
    }

    public int Matcount(string mat)
    {
        int fullstack = 0;
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            Slot currentslot = inventory.transform.GetChild(i).GetComponent<Slot>();
            if (currentslot.item != null && currentslot.item is CraftMat && currentslot.item.itemName == mat)
            {
                fullstack += currentslot.itemAmount;
            }
        }
        return fullstack;
    }

    public void RemoveInventoryMat()
    {
        //removes FIRST material requirement inside recipe
        inventory.RemoveSpecificMat(SelectedRecipeSlot.recipe.material1, SelectedRecipeSlot.recipe.material1quantity);
        if (SelectedRecipeSlot.recipe.material2 != null)
        {
            //removes SECOND material requirement inside recipe
            inventory.RemoveSpecificMat(SelectedRecipeSlot.recipe.material2, SelectedRecipeSlot.recipe.material2quantity);
        }
        if (SelectedRecipeSlot.recipe.material3 != null)
        {
            //removes THIRD material requirement inside recipe
            inventory.RemoveSpecificMat(SelectedRecipeSlot.recipe.material3, SelectedRecipeSlot.recipe.material3quantity);
        }
        if (SelectedRecipeSlot.recipe.material4 != null)
        {
            //removes FOURTH material requirement inside recipe
            inventory.RemoveSpecificMat(SelectedRecipeSlot.recipe.material4, SelectedRecipeSlot.recipe.material4quantity);
        }
        GameController.controller.uiController.RefreshInventory();
    }
}
