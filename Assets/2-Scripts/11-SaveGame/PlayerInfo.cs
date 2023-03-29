using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    //Basic Info
    //Stats
    public int strength, agility, intelligence, vitality, armor, potioneffect;
    public float life, maxlife, meleeDmg, critDmg, critChance, cooldown;
    //Other
    public string playername;
    public int savenumber, level, currentXP, nextLevelXP, lastlevelXP, atributePoints;
    public float positionx;
    public float positiony;
    public float positionz;
    public bool tutorialdone;

    //Equips
    //Weapon
    public string weapontype, weaponsubtype, weaponmain, weaponprimary, weaponsecondary1, weaponsecondary2;
    public int weaponsprite, weapontier, weaponmainvalue, weaponprimaryvalue, weaponsecondary1value, weaponsecondary2value, weaponprice;
    //Shield
    public string shieldtype, shieldsubtype, shieldmain, shieldprimary, shieldsecondary1, shieldsecondary2;
    public int shieldsprite, shieldtier, shieldmainvalue, shieldprimaryvalue, shieldsecondary1value, shieldsecondary2value,shieldprice;
    //Head
    public string headtype, headsubtype, headmain, headprimary, headsecondary1, headsecondary2;
    public int headsprite, headtier, headmainvalue, headprimaryvalue, headsecondary1value, headsecondary2value, headprice;
    //Body
    public string bodytype, bodysubtype, bodymain, bodyprimary, bodysecondary1, bodysecondary2;
    public int bodysprite, bodytier, bodymainvalue, bodyprimaryvalue, bodysecondary1value, bodysecondary2value, bodyprice;
    //Foot
    public string foottype, footsubtype, footmain, footprimary, footsecondary1, footsecondary2;
    public int footsprite, foottier, footmainvalue, footprimaryvalue, footsecondary1value, footsecondary2value, footprice;

    //Inventory Slots
    //Se equipable
    public string[] itemtype = new string[18];
    public string[] itemsubtype = new string[18];
    public string[] itemmain = new string[18];
    public string[] itemprimary = new string[18];
    public string[] itemsecondary1 = new string[18];
    public string[] itemsecondary2 = new string[18];
    public int[] itemsprite = new int[18];
    public int[] itemtier = new int[18];
    public int[] itemmainvalue = new int[18];
    public int[] itemprimaryvalue = new int[18];
    public int[] itemsecondary1value = new int[18];
    public int[] itemsecondary2value = new int[18];
    public int[] itemprice = new int[18];
    //Se craftmat
    public string[] itemname = new string[18];
    public int[] itemmaxstackamount = new int[18];
    public int[] itemamount = new int[18];


}
