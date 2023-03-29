using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Drop : MonoBehaviour
{
    public Item myItem;
    public ParticleSystem pickparticles;
    [SerializeField] private GameObject goldprefab;
    public Animator animator;
    private int random;
    public string deadenemy;

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        GameController.controller.uiController.ShowAction("pickup",true);
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("pickup",false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("pickup", true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player player = other.GetComponent<Player>();
                int randomnum = Random.Range(0, 100);
                if (myItem == null)
                {
                    if (randomnum < 10) //Verifica se criou um item!
                    {
                        myItem = ChooseItemToCreate(deadenemy); // Cria item de acordo com o inimigo que matou
                    }
                    else if (randomnum < 60) //Senão verifica se criou um material!
                    {
                        myItem = DataBase.dataBase.CreateMaterial(deadenemy, "drop2"); //cria um material básico que corresponde ao inimigo morto
                        int randomnum2 = Random.Range(0, 100);
                        if (randomnum2 < 50 && deadenemy!="Tupinim" && deadenemy!="Snake") //Verifica se caiu um drop2 e um drop1 também!
                        {
                            Item myseconditem = new CraftMat();
                            myseconditem= DataBase.dataBase.CreateMaterial(deadenemy, "drop1"); // cria um material complementar
                            if (player.passivesManager.getLootingUnlocked())
                            {
                                int randomnum3 = Random.Range(0, 100);
                                if (randomnum3 <= player.passivesManager.getLootingChance())
                                {
                                    other.GetComponent<Player>().myinventory.AddItem(myseconditem, 2);
                                    //hud mostrar 2x
                                }
                            }
                            else
                            {
                                other.GetComponent<Player>().myinventory.AddItem(myseconditem, 1);
                            }
                        }
                    }
                    else // então é ouro!
                    {
                        int rand = Random.Range(1, 16);
                        GameController.controller.player.GetComponent<Player>().myinventory.AddGold(rand);
                        GameObject goldtext = Instantiate(goldprefab, transform.position, transform.rotation);
                        goldtext.GetComponent<GoldText>().SetGold(rand);
                    }
                }
                if (myItem == null) // Não gerou item dentro do item acima, então gerou gold!
                {
                    StartCoroutine(Destroy());
                }
                else
                {
                    bool picked = false;

                    //DoubleLoot Passive
                    if (myItem is CraftMat && player.passivesManager.getLootingUnlocked())
                    {
                        if (Random.Range(0, 100) <= player.passivesManager.getLootingChance())
                        {
                            picked = other.GetComponent<Player>().myinventory.AddItem(myItem, 2);
                            //hud mostrar 2x
                        }
                    }
                    else
                    {
                        picked = other.GetComponent<Player>().myinventory.AddItem(myItem, 1);
                    }

                    if (picked)
                    {
                        StartCoroutine(Destroy());
                    }
                }
            }
        }
    }

    private Item ChooseItemToCreate(string deadenemy)
    {
        switch (deadenemy)
        {
            case "Tupinim":
            case "Snake":
            case "Bee":
            case "Worm":
            case "Slime":
                //tier 1
                if (Random.Range(0,100)<10) // 10% de chance
                {
                    return DataBase.dataBase.CreateEquipableByTier(2);
                }
                else // 90% de chance
                {
                    return DataBase.dataBase.CreateEquipableByTier(1);
                }
            case "Monk Tree":
            case "Carnivorous Plant":
            case "Skeleton":
            case "Cactus":
                //tier 2
                if (Random.Range(0, 100) < 10) //10% de chance
                {
                    return DataBase.dataBase.CreateEquipableByTier(3);
                }
                else if (Random.Range(0, 100) < 35) // 31,5% de chance
                {
                    return DataBase.dataBase.CreateEquipableByTier(2);
                }
                else // 58,5%
                {
                    return DataBase.dataBase.CreateEquipableByTier(1);
                }
            case "Golem":
            case "Gorila":
                //tier 3
                if (Random.Range(0, 100) < 35) // 35% de chance
                {
                    return DataBase.dataBase.CreateEquipableByTier(3);
                }
                else if (Random.Range(0, 100) < 55) // 35,75% de chance 
                {
                    return DataBase.dataBase.CreateEquipableByTier(2);
                }
                else //29,25% de chance 
                {
                    return DataBase.dataBase.CreateEquipableByTier(1);
                }
            default:
                return DataBase.dataBase.CreateEquipableByTier(1);
        }
    }

    public IEnumerator Destroy()
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        GameController.controller.uiController.ShowAction("pickup",false);
        animator.SetTrigger("open");
        pickparticles.Play();
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }

    public IEnumerator AutoDestroy()
    {
        //Alguma animação, efeito, particula do item desaparecendo? Caso positivo roda aqui.
        yield return new WaitForSeconds(15);
        GameController.controller.uiController.ShowAction("pickup", false);
        Destroy(this.gameObject);
    }
}
