using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : Content
{
    //General
    public string name;
    public string description;
    public string type;
    public int level;
    public bool cansend;
    public int xpReward;
    public int goldReward;
    public GameObject prefab;


    //public Item itemReward;
    public bool isCompleted;
    public bool isMain;
    public QuestMenu npc;
    public Transform location;

    //KillQuest
    public int killObjective;
    public int killCounter = 0;
    public int enemyId;

    //RescueQuest
    public int rescueobjective;
    public int rescuecounter = 0;
    public GameObject mushguard;

    //PickUpQuest
    public string materialtopickup1;
    public int quantitytopickup1;
    public string materialtopickup2;
    public int quantitytopickup2;

    public void CheckKill(int id)
    {
        if (id == enemyId)
        {
            Debug.Log(id + " é igual a " + enemyId + "então tem que contar no contador do questlog");
            killCounter++;
            if (killCounter == killObjective)
            {
                isCompleted = true;
                GameController.controller.uiController.questlogPanel.GetComponent<QuestLog>().CompleteQuest(this);
                GameController.controller.uiController.CompleteQuestAnimationCall(name);
            }
        }
    }

    public void CheckRescue()
    {
        rescuecounter++;
        if (rescuecounter == rescueobjective)
        {
            isCompleted = true;
            Debug.Log("Completed " + name);
            GameController.controller.uiController.questlogPanel.GetComponent<QuestLog>().CompleteQuest(this);
            GameController.controller.uiController.CompleteQuestAnimationCall(name);
        }
    }

    public void CheckPickUp()
    {
        isCompleted = true;
        Debug.Log("Completed " + name);
        GameController.controller.uiController.questlogPanel.GetComponent<QuestLog>().CompleteQuest(this);
        GameController.controller.uiController.CompleteQuestAnimationCall(name);
    }

}
