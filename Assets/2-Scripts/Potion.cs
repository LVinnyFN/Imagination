using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{

    private float timer = 20;
    private Player player;
    private Image timeFill;
    private bool tutorialbool = false;
    public int potionquantity;
    [SerializeField] private GameObject feedlifefull;
    [SerializeField] private GameObject x;

    void Start()
    {
        player = GameController.controller.player.GetComponent<Player>();
        timeFill = GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (potionquantity > 0)
            {
                if (player.currentlife != player.maxlife)
                {
                    if (!player.isDead)
                    {
                        {

                            potionquantity--;
                            UsePotion(player.potioneffect);
                            GameController.controller.uiController.RefreshUI();
                        }
                    }
                    else
                    {
                        //Você está morto!
                    }
                }
                else
                {
                    if (!feedlifefull.activeSelf)
                    {
                    StartCoroutine(FeedLifeFull());
                    }
                }
            }
            else
            {
                if (!x.activeSelf)
                {
                    StartCoroutine(ShowX());
                }
            }
        }
    }

    public void UsePotion(int percentage)
    {
        player.Heal(percentage);
        timer = 0;
    }

    IEnumerator FeedLifeFull()
    {
        feedlifefull.SetActive(true);
        yield return new WaitForSeconds(3);
        feedlifefull.SetActive(false);
    }

    IEnumerator ShowX()
    {
        x.SetActive(true);
        yield return new WaitForSeconds(2);
        x.SetActive(false);
    }

}
