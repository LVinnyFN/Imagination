using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMush : MonoBehaviour
{
    public bool tutstart = false;
    public bool chestdone = false;
    public bool equipped = false;
    public bool beedone = false;
    public Player player;
    public GameObject dialogback;
    public Text dialogtext;
    public bool sentencedone = false;
    private int index = 0;
    public Animator animator;
    public GameObject tutarea;
    public GameObject exclamation;
    [SerializeField] private GameObject close;
    [SerializeField] private Tutchest tutchest;
    [SerializeField] private GameObject tutbee;
    [SerializeField] private GameObject seta;
    [SerializeField] private GameObject godlight;
    [SerializeField] private SaveController saveController;

    public string[] tutoriallines = new string[5] { "1!", "2", "3", "4", "5" };
    public string[] questline = new string[3] { "Now it look like you can beat them all!", "We've heard some mushys went to play near the big tree!", "Bring them home please!" };


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        GameController.controller.uiController.ShowAction("talk", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("talk", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<BoxCollider>().enabled = false;
                player.animator.SetFloat("speed", 0);
                player.isDead = true;
                GameController.controller.uiController.UseMouse(true);
                GameController.controller.uiController.ShowAction("talk", false);
                dialogback.SetActive(true);
                if (beedone)
                {
                    saveController.SaveGame();
                    GameController.controller.tutorialdone = true;
                    exclamation.SetActive(false);
                    StartCoroutine(DestroyTutorial());
                    dialogback.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    dialogtext.text = "Perfect fighting skills! Remember to spend atribute points using 'P' and skill points using 'K' at every level up!";
                }
                else if (equipped)
                {
                    exclamation.SetActive(false);
                    dialogback.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    tutbee.SetActive(true);
                    dialogtext.text = "Time to fight! Press mouse left to attack and mouse right to defend! Goodluck!";
                }
                else if (chestdone)
                {
                    exclamation.SetActive(false);
                    dialogback.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    dialogtext.text = "You must equip the sword and shield! Open inventory with 'I'";
                }
                else
                {
                    //set trigger ficar parado
                    index = 0;
                    StartCoroutine(TypeSentence(tutoriallines[index]));
                }
            }
        }
    }

    public void NextLine()
    {
        //terminou de escrever
        if (sentencedone)
        {
            if (index == tutoriallines.Length - 1)
            {
                tutstart = true;
                close.SetActive(true);
                seta.SetActive(true);
                godlight.SetActive(true);
                exclamation.SetActive(false);
            }
            if (index == tutoriallines.Length)
            {
                StopAllCoroutines();
                //StartCoroutine(TypeSentence("Let's repeat!"));
                dialogtext.text = "Go to the chest right there and grab yourself a sword and a shield and you will feel much more confident!";
                index = 3;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(TypeSentence(tutoriallines[index]));
            }
        }
        else // não terminou de escrever
        {
            StopAllCoroutines();
            dialogtext.text = tutoriallines[index];
            index++;
            sentencedone = true;
        }
    }

    public void Closedialog()
    {
        GameController.controller.uiController.UseMouse(false);
        player.isDead = false;
        dialogback.SetActive(false);
        if (beedone)
        {
            GetComponent<BoxCollider>().enabled = false;

        }
        else if (equipped)
        {
            dialogback.SetActive(false);
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogtext.text = "";
        sentencedone = false;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogtext.text += letter;
            yield return null;
        }
        index++;
        sentencedone = true;
    }

    IEnumerator DestroyTutorial()
    {
        animator.SetTrigger("destroy");
        Closedialog();
        yield return new WaitForSeconds(3);
        GameController.controller.uiController.ShowAction("talk", false);
        Destroy(tutarea.gameObject);
    }
}
