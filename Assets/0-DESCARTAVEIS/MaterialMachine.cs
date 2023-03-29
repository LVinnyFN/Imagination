using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialMachine : MonoBehaviour
{
    [SerializeField] private string machinename;
    [SerializeField] private GameObject materialdrop;
    [SerializeField] private UiController uicontroller;
    [SerializeField] private Inventory inventory;
    [SerializeField] private TextMeshProUGUI materialstext;
    [SerializeField] private ParticleSystem pickupparticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("pickup", true);
            uicontroller.machine = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("pickup", false);
            uicontroller.machine = null;
        }
    }

    public void CreateHoneyCardboard()
    {
        //3x Cardboard
        CraftMat mymat = new CraftMat();
        mymat.maxstackamount = 10;
        mymat.itemName = DataBase.dataBase.myMaterials[0]; 
        mymat.itemIcon = DataBase.dataBase.myMaterialSprites[0];
        mymat.description = DataBase.dataBase.myMaterialDescriptions[0];
        mymat.price = 20;
        inventory.AddItem(mymat, 3);
        
        // 3x Honey
        mymat = new CraftMat();
        mymat.maxstackamount = 10;
        mymat.itemName = DataBase.dataBase.myMaterials[4]; 
        mymat.itemIcon = DataBase.dataBase.myMaterialSprites[4];
        mymat.description = DataBase.dataBase.myMaterialDescriptions[4];
        mymat.price = 20;
        inventory.AddItem(mymat, 3);
    }

    public void CreateMaterials()
    {
        for (int i = 0; i < DataBase.dataBase.myMaterials.Length; i++)
        {
            CraftMat mymat = new CraftMat();
            mymat.maxstackamount = 10;
            mymat.itemName = DataBase.dataBase.myMaterials[i];
            mymat.itemIcon = DataBase.dataBase.myMaterialSprites[i];
            mymat.description = DataBase.dataBase.myMaterialDescriptions[i];
            mymat.price = 20;
            inventory.AddItem(mymat, 10);
        }
        StartCoroutine(GotMaterialsFeedback());
    }

    public IEnumerator GotMaterialsFeedback()
    {
        pickupparticles.Play();
        materialstext.text = "Got it!";
        yield return new WaitForSeconds(2);
        materialstext.text = "Materials\nHere!";
    }
}
