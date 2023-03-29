using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataBase : MonoBehaviour
{
    public static DataBase dataBase;
    public string[] myTypes = new string[5] { "Weapon", "Shield", "Head", "Body", "Foot" };
    public string[] myWeaponTypes = new string[3] { "Cardboard Sword", "Wooden Sword", "Iron Sword" };
    public string[] myShieldTypes = new string[3] { "Cardboard Shield", "Wooden Shield", "Iron Shield" };
    public string[] myHeadTypes = new string[3] { "Cardboard Helmet", "Wooden Helmet", "Iron Helmet" };
    public string[] myBodyTypes = new string[3] { "Cardboard Armor", "Wooden Armor", "Iron Armor" };
    public string[] myFootTypes = new string[3] { "Cardboard Shoes", "Wooden Shoes", "Iron Shoes" };
    public Sprite[] myEquipSprites = new Sprite[15];
    public Sprite[] myTierSprites = new Sprite[3];

    public string[] myMaterials = new string[10] { "Cardboard", "Wood", "Iron", "Goo", "Honey", "Vine", "Bones", "Cactus Flower", "Leather", "Stones" };
    public Sprite[] myMaterialSprites = new Sprite[10];
    public string[] myMaterialDescriptions = new string[10];

    public Sprite[] mySkillSprites = new Sprite[5];
    public string[] mySkillDescriptions = new string[5];
    public Sprite[] myPassiveSprites = new Sprite[6];
    public string[] myPassiveDescriptions = new string[6];

    public string[] myPrimaryAtributes = new string[4] { "Str", "Agi", "Int", "Vit" };
    public List<string> mySecondaryAtributes = new List<string>();

    public int[,] myValuesMin = new int[3, 3] { {  1,  3,  5 },
                                                {  4,  7,  11 },
                                                { 9, 13, 17 } };

    public int[,] myValuesMax = new int[3, 3] { {  2,  4,  6 },
                                                {  6,  9, 13 },
                                                { 12, 16, 20 } };

    private int random;
    //Create Random Seed
    System.Random seed = new System.Random(DateTime.Now.Millisecond);

    void Start()
    {
        dataBase = this;
        // Criação da Lista
        mySecondaryAtributes.Clear();
        mySecondaryAtributes.Add("PExtra");
        mySecondaryAtributes.Add("MExtra");
        mySecondaryAtributes.Add("AExtra");
        mySecondaryAtributes.Add("Life");
        mySecondaryAtributes.Add("CritDmg");
        mySecondaryAtributes.Add("CritChance");
    }

    public CraftMat CreateSpecificMaterial(string material)
    {
        CraftMat mymat = new CraftMat();
        mymat.itemName = material;
        switch (material)
        {
            case "Cardboard":
                mymat.itemIcon = myMaterialSprites[0];
                mymat.description = myMaterialDescriptions[0];
                mymat.price = 20;
                break;
            case "Wood":
                mymat.itemIcon = myMaterialSprites[1];
                mymat.description = myMaterialDescriptions[1];
                mymat.price = 50;
                break;
            case "Iron":
                mymat.itemIcon = myMaterialSprites[2];
                mymat.description = myMaterialDescriptions[0];
                mymat.price = 80;
                break;
            case "Goo":
                mymat.itemIcon = myMaterialSprites[3];
                mymat.description = myMaterialDescriptions[3];
                mymat.price = 20;
                break;
            case "Honey":
                mymat.itemIcon = myMaterialSprites[4];
                mymat.description = myMaterialDescriptions[4];
                mymat.price = 20;
                break;
            case "Vine":
                mymat.itemIcon = myMaterialSprites[5];
                mymat.description = myMaterialDescriptions[5];
                mymat.price = 50;
                break;
            case "Bones":
                mymat.itemIcon = myMaterialSprites[6];
                mymat.description = myMaterialDescriptions[6];
                mymat.price = 50;
                break;
            case "Cactus Flower":
                mymat.itemIcon = myMaterialSprites[7];
                mymat.description = myMaterialDescriptions[7];
                mymat.price = 50;
                break;
            case "Leather":
                mymat.itemIcon = myMaterialSprites[8];
                mymat.description = myMaterialDescriptions[8];
                mymat.price = 80;
                break;
            case "Stones":
                mymat.itemIcon = myMaterialSprites[9];
                mymat.description = myMaterialDescriptions[9];
                mymat.price = 80;
                break;
            default:
                break;
        }
        mymat.maxstackamount = 10;
        return mymat;
    }

    public CraftMat CreateMaterial(string deadenemy, string type)
    {
        CraftMat mymat = new CraftMat();
        switch (deadenemy)
        {
            case "Tupinim":
            case "Snake":
                if (type == "drop2")
                {
                    if (UnityEngine.Random.Range(0, 100) < 80)
                    {
                        mymat=CreateSpecificMaterial("Cardboard");
                    }
                    else
                    {
                        mymat = CreateSpecificMaterial("Wood");
                    }
                }
                else
                {
                    mymat = CreateSpecificMaterial("Cardboard"); // Essa linha não deve acontecer. (só esta aqui para evitar erros
                }
                break;
            case "Bee":
                if (type == "drop2")
                {
                    if (UnityEngine.Random.Range(0, 100) < 80)
                    {
                        mymat = CreateSpecificMaterial("Cardboard");
                    }
                    else
                    {
                        mymat = CreateSpecificMaterial("Wood");
                    }
                }
                else
                {
                    mymat = CreateSpecificMaterial("Honey");
                }
                break;
            case "Worm":
            case "Slime":
                if (type == "drop2")
                {
                    if (UnityEngine.Random.Range(0, 100) < 80)
                    {
                        mymat = CreateSpecificMaterial("Cardboard");
                    }
                    else
                    {
                        mymat = CreateSpecificMaterial("Wood");
                    }
                }
                else
                {
                    mymat = CreateSpecificMaterial("Goo");
                }
                break;
            case "Monk Tree":
            case "Carnivorous Plant":
                if (type == "drop2")
                {
                    if (UnityEngine.Random.Range(0, 100) < 80)
                    {
                        mymat = CreateSpecificMaterial("Wood");
                    }
                    else
                    {
                        mymat = CreateSpecificMaterial("Iron");
                    }
                }
                else
                {
                    mymat = CreateSpecificMaterial("Vine");
                }
                break;
            case "Skeleton":
                if (type == "drop2")
                {
                    if (UnityEngine.Random.Range(0, 100) < 80)
                    {
                        mymat = CreateSpecificMaterial("Wood");
                    }
                    else
                    {
                        mymat = CreateSpecificMaterial("Iron");
                    }
                }
                else
                {
                    mymat = CreateSpecificMaterial("Bones");
                }
                break;
            case "Cactus":
                if (type == "drop2")
                {
                    if (UnityEngine.Random.Range(0, 100) < 80)
                    {
                        mymat = CreateSpecificMaterial("Wood");
                    }
                    else
                    {
                        mymat = CreateSpecificMaterial("Iron");
                    }
                }
                else
                {
                    mymat = CreateSpecificMaterial("Cactus Flower");
                }
                break;
            case "Golem":
                if (type == "drop2")
                {
                    mymat = CreateSpecificMaterial("Iron");
                }
                else
                {
                    mymat = CreateSpecificMaterial("Stones");
                }
                break;
            case "Gorila":
                if (type == "drop2")
                {
                    mymat = CreateSpecificMaterial("Iron");
                }
                else
                {
                    mymat = CreateSpecificMaterial("Leather");
                }
                break;
            default:
                mymat = CreateSpecificMaterial("Cardboard");
                break;
        }
        return mymat;
    }

    public Equipable CreateEquipableByTier(int tier)
    {
        //Create Equipable
        Equipable equipable = new Equipable();
        equipable.tier = tier;
        equipable = ChooseType(tier, equipable);
        return equipable;
    }

    public Equipable CreateSpecificEquipable(int tier, string specificType, string specificSubType)
    {
        //Choose Atributes
        if (specificSubType == "")
        {
            //Create Equipable
            Equipable equipable = new Equipable();
            equipable.tier = tier;

            //Define Type and Subtype
            equipable.type = specificType;
            equipable = ChooseSubType(tier, equipable, equipable.type);
            return equipable;
        }
        else
        {
            //Create Equipable
            Equipable equipable = new Equipable();
            equipable.tier = tier;

            //Define Type and Subtype
            equipable.type = specificType;
            equipable.subType = specificSubType;
            equipable = ChooseMainAtribute(tier, equipable, equipable.subType);
            return equipable;
        }
    }

    public Equipable ChooseType(int tier, Equipable equipable)
    {
        int rand = seed.Next(0, myTypes.Length);
        equipable.type = myTypes[rand];
        ChooseSubType(tier, equipable, equipable.type);
        return equipable;
    }

    public Equipable ChooseSubType(int tier, Equipable equipable, string type)
    {
        int rand;
        switch (type)
        {
            case "Weapon":
                rand = seed.Next(0, myWeaponTypes.Length);
                equipable.subType = myWeaponTypes[rand];
                equipable.price += 30;
                break;
            case "Shield":
                rand = seed.Next(0, myShieldTypes.Length);
                equipable.subType = myShieldTypes[rand];
                equipable.price += 20;
                break;
            case "Head":
                rand = seed.Next(0, myHeadTypes.Length);
                equipable.subType = myHeadTypes[rand];
                equipable.price += 50;
                break;
            case "Body":
                rand = seed.Next(0, myBodyTypes.Length);
                equipable.subType = myBodyTypes[rand];
                equipable.price += 80;
                break;
            case "Foot":
                rand = seed.Next(0, myFootTypes.Length);
                equipable.subType = myFootTypes[rand];
                equipable.price += 20;
                break;
            default:
                break;
        }
        ChooseMainAtribute(tier, equipable, equipable.subType);
        return equipable;
    }

    public Equipable ChooseMainAtribute(int tier, Equipable equipable, string subType)
    {
        equipable.itemName = subType;
        switch (subType)
        {
            case "Cardboard Sword":
                equipable.itemIcon = myEquipSprites[0];
                equipable.price += 5;
                equipable.mainAtribute = "PDmg";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Wooden Sword":
                equipable.itemIcon = myEquipSprites[1];
                equipable.price += 10;
                equipable.mainAtribute = "PDmg";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Iron Sword":
                equipable.itemIcon = myEquipSprites[2];
                equipable.price += 15;
                equipable.mainAtribute = "PDmg";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Cardboard Shield":
                equipable.itemIcon = myEquipSprites[3];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Wooden Shield":
                equipable.itemIcon = myEquipSprites[4];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Iron Shield":
                equipable.itemIcon = myEquipSprites[5];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Cardboard Helmet":
                equipable.itemIcon = myEquipSprites[6];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Wooden Helmet":
                equipable.itemIcon = myEquipSprites[7];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Iron Helmet":
                equipable.itemIcon = myEquipSprites[8];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Cardboard Armor":
                equipable.itemIcon = myEquipSprites[9];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Wooden Armor":
                equipable.itemIcon = myEquipSprites[10];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Iron Armor":
                equipable.itemIcon = myEquipSprites[11];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Cardboard Shoes":
                equipable.itemIcon = myEquipSprites[12];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Wooden Shoes":
                equipable.itemIcon = myEquipSprites[13];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;
            case "Iron Shoes":
                equipable.itemIcon = myEquipSprites[14];
                equipable.mainAtribute = "Armor";
                equipable.mainValue = ChooseValue(tier, equipable, subType);
                break;

            default:
                break;
        }
        ChoosePrimaryAtribute(tier, equipable, subType);
        if (tier >= 2)
        {
            equipable.price += 30;
            equipable.secondaryAtribute1 = ChooseSecondaryAtribute(tier, equipable, subType);
            equipable.secondaryValue1 = ChooseValue(tier, equipable, subType);
            if (tier == 3)
            {
                equipable.price += 60;
                equipable.secondaryAtribute2 = ChooseSecondaryAtribute(tier, equipable, subType);
                equipable.secondayValue2 = ChooseValue(tier, equipable, subType);
                mySecondaryAtributes.Add(equipable.secondaryAtribute2);
            }
            mySecondaryAtributes.Add(equipable.secondaryAtribute1);
        }
        return equipable;
    }

    public void ChoosePrimaryAtribute(int tier, Equipable equipable, string subType)
    {
        if (subType == "Cardboard Sword" || subType == "Wooden Sword" || subType == "Iron Sword")
        {
            int rand0 = seed.Next(0, 3);
            equipable.primaryAtribute = myPrimaryAtributes[rand0];
            equipable.primaryValue = ChooseValue(tier, equipable, subType);
        }
        else
        {
            int rand2 = seed.Next(0, myPrimaryAtributes.Length);
            equipable.primaryAtribute = myPrimaryAtributes[rand2];
            equipable.primaryValue = ChooseValue(tier, equipable, subType);
        }
    }

    public string ChooseSecondaryAtribute(int tier, Equipable equipable, string subType)
    {
        string atribute;
        int rand = seed.Next(0, mySecondaryAtributes.Count);
        if (subType == "Cardboard Sword" || subType == "Woorden Sword" || subType == "Iron Sword")
        {
            do
            {
                rand = seed.Next(0, mySecondaryAtributes.Count);
                atribute = mySecondaryAtributes[rand];
            } while (atribute == "MExtra" || atribute == "AExtra" || atribute == "Life");
            mySecondaryAtributes.RemoveAt(rand);
            return atribute;
        }
        else
        {
            do
            {
                rand = seed.Next(0, mySecondaryAtributes.Count);
                atribute = mySecondaryAtributes[rand];
            } while (atribute == "PExtra" || atribute == "MExtra");
            mySecondaryAtributes.RemoveAt(rand);
            return atribute;
        }

    }

    public string ChooseSecondaryAtribute(int tier, string type)
    {
        switch (seed.Next(0, mySecondaryAtributes.Count))
        {
            case 0:
                string aux0 = mySecondaryAtributes[0];
                mySecondaryAtributes.RemoveAt(0);
                return aux0;
            case 1:
                string aux1 = mySecondaryAtributes[1];
                mySecondaryAtributes.RemoveAt(1);
                return aux1;
            case 2:
                string aux2 = mySecondaryAtributes[2];
                mySecondaryAtributes.RemoveAt(2);
                return aux2;
            case 3:
                string aux3 = mySecondaryAtributes[3];
                mySecondaryAtributes.RemoveAt(3);
                return aux3;
            default:
                return null;
        }
    }

    public int ChooseValue(int tier, Equipable equipable, string subType)
    {
        int value;
        switch (subType)
        {
            case "Cardboard Sword":
            case "Cardboard Shield":
            case "Cardboard Helmet":
            case "Cardboard Armor":
            case "Cardboard Shoes":
                value = seed.Next(myValuesMin[tier - 1, 0], myValuesMax[tier - 1, 0] + 1);
                break;
            case "Wooden Sword":
            case "Wooden Shield":
            case "Wooden Helmet":
            case "Wooden Armor":
            case "Wooden Shoes":
                value = seed.Next(myValuesMin[tier - 1, 1], myValuesMax[tier - 1, 1] + 1);
                break;
            default:
                value = seed.Next(myValuesMin[tier - 1, 2], myValuesMax[tier - 1, 2] + 1);
                break;
        }
        if (value == myValuesMax[tier - 1, 0])
        {
            equipable.price += 5;
        }
        return value;
    }
}
