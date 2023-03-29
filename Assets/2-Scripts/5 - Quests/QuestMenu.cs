using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestMenu : MonoBehaviour
{
    //Quest list
    private List myQuests = new List();
    //Quests that will be in quest list after menu starts
    public Quest[] quests;
    public bool initial = false;
    [SerializeField] private RectTransform contentrect;

    public GameObject questArea;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject buttonAccept;
    [SerializeField] private GameObject buttonDeliver;
    [SerializeField] private UiController uicontroller;
    [SerializeField] private GameObject selectedQuest;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private GameObject rewards;

    //private bool isOpen = false;

    private void Start()
    {
        //Adds created quests in NPC list
        RefreshQuestList();
        

    }

    public void RefreshQuestList()
    {
        //ClearQuestMenu();
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].npc = this;
            if (quests[i].cansend && GameController.controller.player.GetComponent<Player>().level >= quests[i].level && !quests[i].isCompleted)
            {
                Element aux = GameController.controller.uiController.GetQuestLog().GetComponent<QuestLog>().myQuests.first.next;
                bool questexistinquestlog = false;
                while (aux != null) //enquanto tiver quest no questlog
                {
                    Quest auxDoAux = (Quest)aux.myContent;
                    if (auxDoAux == quests[i])
                    {
                        questexistinquestlog = true;
                    }
                    aux = aux.next;
                }
                bool questexistinquestmenu = false;
                for (int j = 0; j < questArea.transform.childCount; j++)
                {
                    Quest questinquestmenu = questArea.transform.GetChild(j).GetComponent<QuestPrefab>().myQuest;
                    if (questinquestmenu==quests[i])
                    {
                        questexistinquestmenu = true;
                    }
                }
                if (!questexistinquestlog && !questexistinquestmenu)
                {
                    AddQuest(quests[i]);
                }
            }
        }
    }

    private void ClearQuestMenu()
    {
        contentrect.sizeDelta = new Vector2(contentrect.sizeDelta.x, 2);
        for (int i = 0; i < questArea.transform.childCount; i++)
        {
            myQuests.Remove(questArea.transform.GetChild(i).GetComponent<QuestPrefab>().myQuest);
            Destroy(questArea.transform.GetChild(i).gameObject);
        }
    }

    public void CompleteQuest()
    {
        rewards.SetActive(false);
        QuestPrefab aux = selectedQuest.GetComponent<QuestPrefab>();
        GameController.controller.player.GetComponent<Player>().GainXP(aux.myQuest.xpReward);
        GameController.controller.player.GetComponent<Player>().myinventory.AddGold(aux.myQuest.goldReward);
        GameController.controller.uiController.questlogPanel.gameObject.GetComponent<QuestLog>().selectedQuest = selectedQuest.GetComponent<QuestPrefab>().mirror; //seleciona a quest mirror dentro do questlog
        switch (aux.myQuest.type)
        {
            case "rescue":
                Destroy(aux.myQuest.mushguard.GetComponent<MushroomGuard>().mywall);
                Destroy(aux.myQuest.mushguard);
                if (aux.myQuest == quests[0])
                {
                    quests[1].cansend = true;//habilita a prox quest de rescue
                    quests[8].cansend = true;//habilita as prox quests de pickup
                    quests[9].cansend = true;//habilita as prox quests de pickup
                    quests[20].cansend = true;//habilita a prox quest de kill                    
                }
                if (aux.myQuest == quests[1])
                {
                    quests[2].cansend = true;
                    quests[10].cansend = true;//habilita as prox quests de pickup
                    quests[11].cansend = true;//habilita as prox quests de pickup
                    quests[21].cansend = true;//habilita as prox quests de kill                    
                    quests[22].cansend = true;//habilita as prox quests de kill                    
                    quests[23].cansend = true;//habilita as prox quests de kill                    
                }
                if (aux.myQuest == quests[2])
                {
                    quests[3].cansend = true;
                    quests[12].cansend = true;//habilita as prox quests de pickup
                    quests[13].cansend = true;//habilita as prox quests de pickup
                    quests[14].cansend = true;//habilita as prox quests de pickup
                    quests[24].cansend = true;//habilita as prox quests de kill                    
                    quests[25].cansend = true;//habilita as prox quests de kill                    
                }
                break;
            case "kill":
                //Quando entrega uma quest de kill...
                break;
            case "pickup":
                //quando entrega um quest de pickup...
                if (aux.myQuest.materialtopickup1 != "")
                {
                    GameController.controller.player.GetComponent<Player>().myinventory.RemoveSpecificMat(aux.myQuest.materialtopickup1, aux.myQuest.quantitytopickup1);
                }
                if (aux.myQuest.materialtopickup2 != "")
                {
                    GameController.controller.player.GetComponent<Player>().myinventory.RemoveSpecificMat(aux.myQuest.materialtopickup2, aux.myQuest.quantitytopickup2);
                }
                break;
            default:
                break;
        }
        GameController.controller.uiController.questlogPanel.gameObject.GetComponent<QuestLog>().RemoveQuest(); //manda o questlog deletar a quest mirror
        RemoveQuest(); //Destroi a quest na lista do NPC (painel)
        RefreshQuestList();
    }

    public GameObject AddQuest(Quest quest)
    {
        contentrect.sizeDelta = new Vector2(contentrect.sizeDelta.x, contentrect.sizeDelta.y + 12.5f);
        GameObject go = Instantiate(questPrefab, questArea.transform);
        go.GetComponent<QuestPrefab>().myQuest = quest;
        go.GetComponentInChildren<TextMeshProUGUI>().text = quest.name;
        myQuests.Add(quest);

        if (quest.isMain)
        {
            go.GetComponent<QuestPrefab>().mylittlestar.SetActive(true);
        }
        if (!quest.isCompleted)
            go.GetComponent<QuestPrefab>().myQuest.prefab = go;

        return go;
    }
    public void RemoveQuest()
    {
        contentrect.sizeDelta = new Vector2(contentrect.sizeDelta.x, contentrect.sizeDelta.y - 12.5f);
        Destroy(selectedQuest);
        description.text = null;
        buttonAccept.SetActive(false);
        myQuests.Remove(selectedQuest.GetComponent<QuestPrefab>().myQuest);
    }

    public void SendQuest()
    {
        rewards.SetActive(false);
        Quest quest = selectedQuest.GetComponent<QuestPrefab>().myQuest;
        transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<QuestLog>().AddQuest(quest);
        RemoveQuest();
    }
    public void SelectQuest(GameObject questPrefab)
    {
        selectedQuest = questPrefab;
        if (selectedQuest.GetComponent<QuestPrefab>().myQuest.isCompleted)
        {
            buttonAccept.SetActive(false);
            buttonDeliver.SetActive(true);
            rewards.SetActive(true);
        }
        else
        {
            buttonDeliver.SetActive(false);
            buttonAccept.SetActive(true);
            rewards.SetActive(true);
        }
        description.text = questPrefab.GetComponent<QuestPrefab>().myQuest.description;
        description.text = description.text.Replace("\\n", "\n");
        description.text = description.text.Replace("playername", GameController.controller.playername);
        rewards.transform.GetChild(2).GetComponent<Text>().text = questPrefab.GetComponent<QuestPrefab>().myQuest.xpReward.ToString();
        rewards.transform.GetChild(4).GetComponent<Text>().text = questPrefab.GetComponent<QuestPrefab>().myQuest.goldReward.ToString();
    }

    //public void SwitchMenu(ref bool canrotate)
    //{
    //    if (isOpen == false)
    //    {
    //        OpenMenu();
    //        canrotate = false;
    //    }
    //    else
    //    {
    //        CloseMenu();
    //        canrotate = true;
    //    }

    //}
    //public void OpenMenu()
    //{
    //    this.gameObject.SetActive(true);
    //    isOpen = true;
    //}
    //public void CloseMenu()
    //{
    //    this.gameObject.SetActive(false);
    //    description.text = null;
    //    buttonAccept.SetActive(false);
    //    isOpen = false;
    //}
}
