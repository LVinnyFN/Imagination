using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutchest : MonoBehaviour
{
    [SerializeField] private TutorialMush tutmush;
    public Item myItem;
    public ParticleSystem pickparticles;
    public Animator animator;
    public GameObject seta;
    public bool canopen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutmush.tutstart)
            {
                GameController.controller.uiController.ShowAction("pickup", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("pickup", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && tutmush.tutstart)
            {
                tutmush.chestdone = true;
                tutmush.exclamation.SetActive(true);
                seta.SetActive(false);
                tutmush.GetComponent<BoxCollider>().enabled = true;
                GetComponent<BoxCollider>().enabled = false;

                //Creating Initial Wooden Sword
                Equipable myEquip1 = new Equipable();
                myEquip1.itemName = "Cardboard Sword";
                myEquip1.mainAtribute = "PDmg";
                myEquip1.mainValue = 1;
                myEquip1.price = 1;
                myEquip1.primaryAtribute = "Str";
                myEquip1.primaryValue = 1;
                myEquip1.subType = "Cardboard Sword";
                myEquip1.tier = 1;
                myEquip1.type = "Weapon";
                myEquip1.itemIcon = DataBase.dataBase.myEquipSprites[0];

                myItem = myEquip1;
                GameController.controller.uiController.ItemPickedFeedback(myEquip1);
                other.GetComponent<Player>().myinventory.AddItem(myItem, 1);
                

                //Creating Initial Wooden Shield
                Equipable myEquip2 = new Equipable();
                myEquip2.itemName = "Cardboard Shield";
                myEquip2.mainAtribute = "Armor";
                myEquip2.mainValue = 1;
                myEquip2.price = 1;
                myEquip2.primaryAtribute = "Vit";
                myEquip2.primaryValue = 1;
                myEquip2.subType = "Cardboard Shield";
                myEquip2.tier = 1;
                myEquip2.type = "Shield";
                myEquip2.itemIcon = DataBase.dataBase.myEquipSprites[3];

                myItem = myEquip2;

                bool picked = other.GetComponent<Player>().myinventory.AddItem(myItem, 1);
                if (picked)
                {
                    StartCoroutine(Destroy());
                }
                animator.SetTrigger("open");
            }
        }
    }


    public IEnumerator Destroy()
    {
        GameController.controller.uiController.ShowAction("pickup", false);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        pickparticles.Play();
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }
}
