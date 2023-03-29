using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipes: ScriptableObject
{
    public string type;
    public string subtype;
    public Sprite icon;
    public int tier;
    public int price;
    public string material1;
    public int material1quantity;
    public string material2=null;
    public int material2quantity;
    public string material3=null;
    public int material3quantity;
    public string material4 = null;
    public int material4quantity;

}
