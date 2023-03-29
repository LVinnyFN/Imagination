using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscInicial : MonoBehaviour
{
    [SerializeField] private GameObject optionspanel;
    [SerializeField] private GameObject creditspanel;

    private void Start()
    {
       // GameController.controller.transform.GetChild(0).GetComponent<AudioSource>().clip = GameController.controller.myaudios[0];
       // GameController.controller.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionspanel.activeSelf)
            {
                optionspanel.SetActive(false);
            }
            else if (creditspanel.activeSelf)
            {
                creditspanel.SetActive(false);
            }
            else
            {
                optionspanel.SetActive(true);
            }
        }
    }

    public void FecharJogo()
    {
        GameController.controller.ExitGame();
    }
}
