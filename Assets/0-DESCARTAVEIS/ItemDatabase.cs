using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> itemDatabase = new List<Item>();

    
    //Searches an item by its ID
    public Item itemById(int id)
    {
        Item returnedItem = itemDatabase[id];
        return returnedItem;
    }
}
