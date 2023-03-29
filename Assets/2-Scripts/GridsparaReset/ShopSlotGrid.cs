using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlotGrid : MonoBehaviour
{
    [SerializeField] private GameObject blackinvcopy;
    [SerializeField] private GameObject shopinvcopy;
    [SerializeField] private Shop shopScript;
    [SerializeField] private Inventory inventory;


    public void ResetSelected()
    {
        if (GameController.controller.uiController.shopNPC != null)
        {
            shopScript = GameObject.FindObjectOfType<Shop>();
            shopScript.SelectShopSlot(null);
            inventory.selectedSlot = null;
            if (GameController.controller.uiController.shopNPC.CompareTag("Blacksmith"))
            {
                for (int i = 0; i < blackinvcopy.transform.childCount; i++)
                {
                    blackinvcopy.transform.GetChild(i).GetComponent<InvCopySlot>().selectedborder.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < shopinvcopy.transform.childCount; i++)
                {
                    shopinvcopy.transform.GetChild(i).GetComponent<InvCopySlot>().selectedborder.SetActive(false);
                }
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ShopSlot>().selectedborder.SetActive(false);
        }
    }
}
