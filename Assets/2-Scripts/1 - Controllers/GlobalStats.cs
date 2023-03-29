using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GlobalStats : MonoBehaviour
{

    //KillCounters

    private int[] killCounter = new int[13];
    private int[] materialCounter = new int[10];

    private void Start()
    {
        GameController.controller.globalStats = this;
    }

    public void EnemyKilled(int id)
    {
        killCounter[id]++;
        Debug.Log("Morreram: " + killCounter[id] + "inimigos do ID: " + id);

        Element aux = GameController.controller.uiController.GetQuestLog().GetComponent<QuestLog>().myQuests.first.next;

        while (aux != null)
        {
            Quest auxDoAux = (Quest)aux.myContent;
            if (auxDoAux.type.Equals("kill"))
            {
                auxDoAux.CheckKill(id); //manda a quest no questlog checar que matou mais um inimigo desse tipo (id) 
            }
            aux = aux.next;
        }
    }

    public void MushRescued()
    {
        Element aux = GameController.controller.uiController.GetQuestLog().GetComponent<QuestLog>().myQuests.first.next;
        while (aux != null)
        {
            Quest auxDoAux = (Quest)aux.myContent;
            if (auxDoAux.type.Equals("rescue"))
            {
                auxDoAux.CheckRescue(); //manda a quest no questlog checar que resgatou mais um cogumelo 
            }
            aux = aux.next;
        }
    }

    public void MaterialPickUp(string materialname)
    {
        switch (materialname)
        {
            case "Cardboard":
                materialCounter[0]++;
                break;
            case "Wood":
                materialCounter[1]++;
                break;
            case "Iron":
                materialCounter[2]++;
                break;
            case "Goo":
                materialCounter[3]++;
                break;
            case "Honey":
                materialCounter[4]++;
                break;
            case "Vine":
                materialCounter[5]++;
                break;
            case "Bones":
                materialCounter[6]++;
                break;
            case "CactusFlower":
                materialCounter[7]++;
                break;
            case "Leather":
                materialCounter[8]++;
                break;
            case "Stones":
                materialCounter[9]++;
                break;
            default:
                break;
        }
    }
}
