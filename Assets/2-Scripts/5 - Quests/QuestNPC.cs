using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    public QuestMenu QuestMenu;
    [SerializeField] private UiController uicontroller;
    [SerializeField] private GameObject exclamation;
    [SerializeField] private GameObject interrogation;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SetOverHead();
    }

    public void SetOverHead()
    {
        if (QuestMenu.initial)
        {
            if (QuestMenu.questArea.transform.childCount > 0)
            {
                exclamation.SetActive(false);
                interrogation.SetActive(true);
                for (int i = 0; i < QuestMenu.questArea.transform.childCount; i++)
                {
                    if (QuestMenu.questArea.transform.GetChild(i).GetComponent<QuestPrefab>().myQuest.isCompleted)
                    {
                        exclamation.SetActive(true);
                        interrogation.SetActive(false);
                    }
                }
            }
            else
            {
                interrogation.SetActive(false);
                exclamation.SetActive(false);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //mostrar o action de talk
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetTrigger("talk");
            }
        }
    }
    //Checks for player collisions in "interaction area"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uicontroller.ShowAction("talk", true);
            //animator.GetComponent<Animator>().SetBool("talking", true);
            uicontroller.questMenu = QuestMenu;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uicontroller.ShowAction("talk", false);
            //animator.SetBool("talking", false);
            GameController.controller.uiController.CloseAll();
            uicontroller.questMenu = null;
        }
    }

}
