using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAdd : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemDatabase database;
    public Inventory inventory;
    void Start()
    {
        
    }

    //Adds items to the inventory when the respective key is pressed (a,b,c,d,e)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inventory.AddItem(database.itemById(0), 1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.AddItem(database.itemById(1), 1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            inventory.AddItem(database.itemById(2), 10);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            inventory.AddItem(database.itemById(3), 10);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(database.itemById(4), 10);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            inventory.goldAmount = inventory.goldAmount + 50000;
        }
    }
}
