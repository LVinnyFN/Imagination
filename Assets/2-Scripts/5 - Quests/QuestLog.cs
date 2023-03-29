using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestLog : MonoBehaviour
{
    //Quest list
    public List myQuests = new List();

    [SerializeField] private RectTransform contentrectmain;
    [SerializeField] private RectTransform contentrectside;

    [SerializeField] private GameObject mainQuestGrid;
    [SerializeField] private GameObject sideQuestGrid;
    public TextMeshProUGUI description;
    public GameObject buttonCancel;

    public GameObject selectedQuest;

    [SerializeField] private GameObject questPrefab;

    private bool isOpen;

    public void AddQuest(Quest quest)
    {
        GameObject go;        
        if (quest.isMain)
        {
            contentrectmain.sizeDelta = new Vector2(contentrectmain.sizeDelta.x, contentrectmain.sizeDelta.y + 12.5f);
            go = Instantiate(questPrefab, mainQuestGrid.transform);
        }
        else
        {
            contentrectside.sizeDelta = new Vector2(contentrectside.sizeDelta.x, contentrectside.sizeDelta.y + 12.5f);
            go = Instantiate(questPrefab, sideQuestGrid.transform);
        }
        go.GetComponent<QuestPrefab>().myQuest = quest;
        go.GetComponentInChildren<TextMeshProUGUI>().text = quest.name;
        myQuests.Add(quest);

        go.GetComponent<QuestPrefab>().myQuest.prefab = go;

        GameController.controller.player.GetComponent<Player>().myinventory.CheckPickUpQuests();
    }
    public void RemoveQuest()
    {
        Destroy(selectedQuest);
        description.text = null;
        buttonCancel.SetActive(false);
        myQuests.Remove(selectedQuest.GetComponent<QuestPrefab>().myQuest);
        if (selectedQuest.GetComponent<QuestPrefab>().myQuest.isMain)
        {
            contentrectmain.sizeDelta = new Vector2(contentrectmain.sizeDelta.x, contentrectmain.sizeDelta.y - 12.5f);
        }
        else
        {
            contentrectside.sizeDelta = new Vector2(contentrectside.sizeDelta.x, contentrectside.sizeDelta.y - 12.5f);
        }
    }
    public void SendQuest()
    {
        Quest quest = selectedQuest.GetComponent<QuestPrefab>().myQuest;
        quest.npc.GetComponent<QuestMenu>().AddQuest(quest);
        RemoveQuest();
    }

    //Send a completed quest back to its NPC
    public void CompleteQuest(Quest quest)
    {
        GameObject aux = quest.npc.GetComponent<QuestMenu>().AddQuest(quest);
        aux.GetComponent<QuestPrefab>().mirror = quest.prefab;
        if (quest.type=="rescue")
        {
            quest.description = "Save the mushrooms!";
            aux.GetComponent<QuestPrefab>().myQuest.description="Completed!!!";
        }
        else if (quest.type=="kill")
        {
            aux.GetComponent<QuestPrefab>().myQuest.description = "Completed! I MUST go back and DELIVER it to that nice mushroom!";
            quest.description = "Great! Kill them all! Muahahaha! Ops.... Never mind!";
        }
        else
        {
            aux.GetComponent<QuestPrefab>().myQuest.description = "Completed! I MUST go back and DELIVER it to that nice mushroom!";
            quest.description = "Well done kid, I knew I could count on you!";
        }
    }

    public void SelectQuest(GameObject questPrefab)
    {
        if(questPrefab.GetComponent<QuestPrefab>().myQuest.isCompleted == false)
            buttonCancel.SetActive(true);

        selectedQuest = questPrefab;       
        description.text = questPrefab.GetComponent<QuestPrefab>().myQuest.description;
        description.text = description.text.Replace("\\n", "\n");
        description.text = description.text.Replace("playername", GameController.controller.playername);
    }

    public void CheckPickUpQuest()
    {
    }


    //public void SwitchMenu()
    //{
    //    if (isOpen == false)
    //        OpenMenu();
    //    else
    //        CloseMenu();
    //}
    //public void OpenMenu()
    //{
    //    this.gameObject.SetActive(true);
    //    isOpen = true;
    //}
    //public void CloseMenu()
    //{
    //    this.gameObject.SetActive(false);
    //    selectedQuest = null;
    //    description.text = null;
    //    buttonCancel.SetActive(false);
    //    isOpen = false;
    //}
}
