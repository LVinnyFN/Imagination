using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    //Crafting craftingScript;

    public Item item;
    Image icon;

    private void Start()
    {
        if (item == null)
        {
            Destroy(gameObject);
        }
        //craftingScript = GameObject.FindObjectOfType<Crafting>();
        icon = GetComponent<Image>();
        icon.sprite = item.itemIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //craftingScript.CraftItem(item);
    }

    private void Update()
    {

    }
}
