using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvCopyGrid : MonoBehaviour
{
    private Shop shopScript;
    [SerializeField] private GameObject blackshop;
    [SerializeField] private GameObject shopshop;

    public void ResetSelected()
    {
        if (GameController.controller.uiController.shopNPC != null)
        {
            shopScript = GameObject.FindObjectOfType<Shop>();
            shopScript.SelectShopSlot(null);
            if (GameController.controller.uiController.shopNPC.CompareTag("Blacksmith"))
            {
                for (int i = 0; i < blackshop.transform.childCount; i++)
                {
                    blackshop.transform.GetChild(i).GetComponent<ShopSlot>().selectedborder.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < shopshop.transform.childCount; i++)
                {
                    shopshop.transform.GetChild(i).GetComponent<ShopSlot>().selectedborder.SetActive(false);
                }
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<InvCopySlot>().selectedborder.SetActive(false);
        }
    }
}
