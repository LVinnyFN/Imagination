using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopNPC : MonoBehaviour
{
    //Unlock Quest
    public bool unlockquestdone = false;
    [SerializeField] private TextMeshProUGUI buttontext;
    [SerializeField] private TextMeshProUGUI dialogtext;
    public string[] shopdialog = new string[3];
    [SerializeField] private Inventory inventory;

    //SHOP
    public GameObject shopMenu;
    [SerializeField] private UiController uicontroller;
    [SerializeField] private GameObject inventoryCopy;
    [SerializeField] private Text goldTextCopy;
    [SerializeField] private Text goldTextCopyCraft;
    [SerializeField] private Text refreshCostText;

    //Checks for player collisions in "interaction area"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("talk", true);
            uicontroller.shopMenu = shopMenu;
            uicontroller.shopNPC = this;
            uicontroller.inventoryCopy = inventoryCopy;
            uicontroller.goldtextCopy = goldTextCopy;
            uicontroller.refreshCostText = refreshCostText;
            uicontroller.goldtextCopyCraft = goldTextCopyCraft;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("talk", false);
            uicontroller.inventoryCopy = null;
            uicontroller.goldtextCopy = null;
            uicontroller.refreshCostText = null;
            uicontroller.goldtextCopyCraft = null;
            uicontroller.CloseAll();
            uicontroller.shopMenu = null;
        }
    }

    public void CompleteUnlockQuest()
    {
        if (transform.CompareTag("Blacksmith"))
        {
            if (!unlockquestdone)
            {
                if (inventory.CheckSpecificMat("Cardboard", 1))
                {
                    StopAllCoroutines();
                    unlockquestdone = true;
                    inventory.RemoveSpecificMat("Cardboard", 1);
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(shopdialog[2]));
                    buttontext.text = "Acess Shop!";
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(shopdialog[1]));
                }
            }
            else
            {
                uicontroller.CloseAll();
                uicontroller.ShowAction("talk", true);
            }
        }
        else
        {
            if (!unlockquestdone)
            {
                if (inventory.CheckSpecificMat("Honey", 1))
                {
                    StopAllCoroutines();
                    unlockquestdone = true;
                    inventory.RemoveSpecificMat("Honey", 1);
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(shopdialog[2]));
                    buttontext.text = "Acess Shop!";
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(shopdialog[1]));
                }
            }
            else
            {
                uicontroller.CloseAll();
                uicontroller.ShowAction("talk", true);
            }
        }
    }

    public void WriteQuest()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(shopdialog[0]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogtext.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogtext.text += letter;
            yield return null;
        }
    }
}
