using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private Animator animator;

    public void Start()
    {
        GameController.controller.transform.GetChild(0).GetComponent<AudioSource>().clip = GameController.controller.myaudios[4];
        GameController.controller.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    public void StartGame()
    {
        fade.StartFading(3);
    }

    public void StopAnim()
    {
        animator.SetTrigger("stopanim");
    }
}
